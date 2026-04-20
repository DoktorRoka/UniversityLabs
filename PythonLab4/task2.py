import sqlite3
import hashlib
import json
import tkinter as tk
from tkinter import ttk, messagebox, filedialog


DB_NAME = "library.db"


def init_db():
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("""CREATE TABLE IF NOT EXISTS users (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        login TEXT UNIQUE NOT NULL,
        password TEXT NOT NULL
    )""")
    c.execute("""CREATE TABLE IF NOT EXISTS authors (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        name TEXT NOT NULL,
        country TEXT,
        years TEXT
    )""")
    c.execute("""CREATE TABLE IF NOT EXISTS books (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        author_id INTEGER,
        title TEXT NOT NULL,
        pages INTEGER,
        publisher TEXT,
        year INTEGER,
        FOREIGN KEY (author_id) REFERENCES authors(id)
    )""")
    admin_hash = hashlib.sha1("admin".encode()).hexdigest()
    try:
        c.execute("INSERT INTO users (login, password) VALUES (?, ?)", ("admin", admin_hash))
    except sqlite3.IntegrityError:
        pass
    conn.commit()
    conn.close()


def check_login(login, password):
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("SELECT password FROM users WHERE login=?", (login,))
    row = c.fetchone()
    conn.close()
    if row is None:
        return False
    return row[0] == hashlib.sha1(password.encode()).hexdigest()


def get_authors():
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("SELECT id, name, country, years FROM authors")
    rows = c.fetchall()
    conn.close()
    return rows


def get_books():
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("""SELECT books.id, authors.name, books.title, books.pages, books.publisher, books.year
                 FROM books LEFT JOIN authors ON books.author_id=authors.id""")
    rows = c.fetchall()
    conn.close()
    return rows


def add_author(name, country, years):
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("INSERT INTO authors (name, country, years) VALUES (?, ?, ?)", (name, country, years))
    conn.commit()
    conn.close()


def add_book(author_id, title, pages, publisher, year):
    conn = sqlite3.connect(DB_NAME)
    c = conn.cursor()
    c.execute("INSERT INTO books (author_id, title, pages, publisher, year) VALUES (?, ?, ?, ?, ?)",
              (author_id, title, pages, publisher, year))
    conn.commit()
    conn.close()


def export_author_json(author, filepath):
    data = {"name": author[1], "country": author[2] or "", "years": author[3] or ""}
    if author[3] and "-" in author[3]:
        parts = author[3].split("-")
        try:
            data["years"] = [int(parts[0].strip()), int(parts[1].strip())]
        except ValueError:
            pass
    with open(filepath, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)


def export_author_xml(author, filepath):
    name = author[1] or ""
    country = author[2] or ""
    years = author[3] or ""
    born = ""
    died = ""
    if years and "-" in years:
        parts = years.split("-")
        born = parts[0].strip()
        died = parts[1].strip()
    xml = f"""<author>
    <name>{name}</name>
    <country>{country}</country>
    <years born="{born}" died="{died}"/>
</author>"""
    with open(filepath, "w", encoding="utf-8") as f:
        f.write(xml)


def parse_author_json(filepath):
    with open(filepath, "r", encoding="utf-8") as f:
        data = json.load(f)
    years = data.get("years", "")
    if isinstance(years, list) and len(years) == 2:
        years = f"{years[0]}-{years[1]}"
    return data.get("name", ""), data.get("country", ""), str(years)


def parse_author_xml(filepath):
    import xml.etree.ElementTree as ET
    tree = ET.parse(filepath)
    root = tree.getroot()
    name = root.find("name").text or ""
    country = root.find("country").text or ""
    years_elem = root.find("years")
    years = ""
    if years_elem is not None:
        born = years_elem.get("born", "")
        died = years_elem.get("died", "")
        if born and died:
            years = f"{born}-{died}"
    return name, country, years


class LoginWindow:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("Library - Login")
        self.root.geometry("300x180")
        self.root.resizable(False, False)

        tk.Label(self.root, text="Login:").grid(row=0, column=0, padx=10, pady=10, sticky="e")
        self.login_entry = tk.Entry(self.root)
        self.login_entry.grid(row=0, column=1, padx=10, pady=10)

        tk.Label(self.root, text="Password:").grid(row=1, column=0, padx=10, pady=10, sticky="e")
        self.pass_entry = tk.Entry(self.root, show="*")
        self.pass_entry.grid(row=1, column=1, padx=10, pady=10)

        tk.Button(self.root, text="Login", command=self.on_login).grid(row=2, column=0, columnspan=2, pady=10)

    def on_login(self):
        login = self.login_entry.get().strip()
        password = self.pass_entry.get().strip()
        if not login or not password:
            messagebox.showwarning("Warning", "Fill in all fields")
            return
        if check_login(login, password):
            self.root.destroy()
            app = LibraryApp()
            app.run()
        else:
            messagebox.showerror("Error", "Invalid login or password")

    def run(self):
        self.root.mainloop()


class LibraryApp:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("Library Information System")
        self.root.geometry("800x500")

        notebook = ttk.Notebook(self.root)
        notebook.pack(fill="both", expand=True, padx=5, pady=5)

        self.authors_tab = tk.Frame(notebook)
        self.books_tab = tk.Frame(notebook)
        self.add_tab = tk.Frame(notebook)

        notebook.add(self.authors_tab, text="Authors")
        notebook.add(self.books_tab, text="Books")
        notebook.add(self.add_tab, text="Add")

        self._build_authors_tab()
        self._build_books_tab()
        self._build_add_tab()

    def _build_authors_tab(self):
        self.authors_tree = ttk.Treeview(self.authors_tab,
                                         columns=("id", "name", "country", "years"),
                                         show="headings", height=15)
        self.authors_tree.heading("id", text="ID")
        self.authors_tree.heading("name", text="Name")
        self.authors_tree.heading("country", text="Country")
        self.authors_tree.heading("years", text="Years")
        self.authors_tree.column("id", width=40)
        self.authors_tree.column("name", width=200)
        self.authors_tree.column("country", width=150)
        self.authors_tree.column("years", width=120)
        self.authors_tree.pack(fill="both", expand=True, padx=5, pady=5)

        btn_frame = tk.Frame(self.authors_tab)
        btn_frame.pack(fill="x", padx=5, pady=5)
        tk.Button(btn_frame, text="Refresh", command=self._refresh_authors).pack(side="left", padx=5)
        tk.Button(btn_frame, text="Export JSON", command=lambda: self._export_author("json")).pack(side="left", padx=5)
        tk.Button(btn_frame, text="Export XML", command=lambda: self._export_author("xml")).pack(side="left", padx=5)

        self._refresh_authors()

    def _build_books_tab(self):
        self.books_tree = ttk.Treeview(self.books_tab,
                                       columns=("id", "author", "title", "pages", "publisher", "year"),
                                       show="headings", height=15)
        self.books_tree.heading("id", text="ID")
        self.books_tree.heading("author", text="Author")
        self.books_tree.heading("title", text="Title")
        self.books_tree.heading("pages", text="Pages")
        self.books_tree.heading("publisher", text="Publisher")
        self.books_tree.heading("year", text="Year")
        self.books_tree.column("id", width=40)
        self.books_tree.column("author", width=150)
        self.books_tree.column("title", width=200)
        self.books_tree.column("pages", width=60)
        self.books_tree.column("publisher", width=120)
        self.books_tree.column("year", width=60)
        self.books_tree.pack(fill="both", expand=True, padx=5, pady=5)

        tk.Button(self.books_tab, text="Refresh", command=self._refresh_books).pack(padx=5, pady=5)

        self._refresh_books()

    def _build_add_tab(self):
        add_notebook = ttk.Notebook(self.add_tab)
        add_notebook.pack(fill="both", expand=True, padx=5, pady=5)

        author_frame = tk.Frame(add_notebook)
        book_frame = tk.Frame(add_notebook)
        add_notebook.add(author_frame, text="Add Author")
        add_notebook.add(book_frame, text="Add Book")

        tk.Label(author_frame, text="Name:").grid(row=0, column=0, padx=5, pady=5, sticky="e")
        self.a_name = tk.Entry(author_frame, width=40)
        self.a_name.grid(row=0, column=1, padx=5, pady=5)

        tk.Label(author_frame, text="Country:").grid(row=1, column=0, padx=5, pady=5, sticky="e")
        self.a_country = tk.Entry(author_frame, width=40)
        self.a_country.grid(row=1, column=1, padx=5, pady=5)

        tk.Label(author_frame, text="Years:").grid(row=2, column=0, padx=5, pady=5, sticky="e")
        self.a_years = tk.Entry(author_frame, width=40)
        self.a_years.grid(row=2, column=1, padx=5, pady=5)

        tk.Button(author_frame, text="Add Author", command=self._add_author).grid(row=3, column=0, columnspan=2, pady=10)
        tk.Button(author_frame, text="Import from File", command=self._import_author).grid(row=4, column=0, columnspan=2, pady=5)

        tk.Label(book_frame, text="Author ID:").grid(row=0, column=0, padx=5, pady=5, sticky="e")
        self.b_author_id = tk.Entry(book_frame, width=40)
        self.b_author_id.grid(row=0, column=1, padx=5, pady=5)

        tk.Label(book_frame, text="Title:").grid(row=1, column=0, padx=5, pady=5, sticky="e")
        self.b_title = tk.Entry(book_frame, width=40)
        self.b_title.grid(row=1, column=1, padx=5, pady=5)

        tk.Label(book_frame, text="Pages:").grid(row=2, column=0, padx=5, pady=5, sticky="e")
        self.b_pages = tk.Entry(book_frame, width=40)
        self.b_pages.grid(row=2, column=1, padx=5, pady=5)

        tk.Label(book_frame, text="Publisher:").grid(row=3, column=0, padx=5, pady=5, sticky="e")
        self.b_publisher = tk.Entry(book_frame, width=40)
        self.b_publisher.grid(row=3, column=1, padx=5, pady=5)

        tk.Label(book_frame, text="Year:").grid(row=4, column=0, padx=5, pady=5, sticky="e")
        self.b_year = tk.Entry(book_frame, width=40)
        self.b_year.grid(row=4, column=1, padx=5, pady=5)

        tk.Button(book_frame, text="Add Book", command=self._add_book).grid(row=5, column=0, columnspan=2, pady=10)

    def _refresh_authors(self):
        for item in self.authors_tree.get_children():
            self.authors_tree.delete(item)
        for row in get_authors():
            self.authors_tree.insert("", "end", values=row)

    def _refresh_books(self):
        for item in self.books_tree.get_children():
            self.books_tree.delete(item)
        for row in get_books():
            self.books_tree.insert("", "end", values=row)

    def _add_author(self):
        name = self.a_name.get().strip()
        country = self.a_country.get().strip()
        years = self.a_years.get().strip()
        if not name:
            messagebox.showwarning("Warning", "Author name is required")
            return
        add_author(name, country, years)
        messagebox.showinfo("Success", "Author added")
        self.a_name.delete(0, "end")
        self.a_country.delete(0, "end")
        self.a_years.delete(0, "end")
        self._refresh_authors()

    def _add_book(self):
        try:
            author_id = int(self.b_author_id.get().strip())
        except ValueError:
            messagebox.showwarning("Warning", "Author ID must be a number")
            return
        title = self.b_title.get().strip()
        if not title:
            messagebox.showwarning("Warning", "Book title is required")
            return
        pages = self.b_pages.get().strip() or None
        publisher = self.b_publisher.get().strip()
        year = self.b_year.get().strip() or None
        add_book(author_id, title, pages, publisher, year)
        messagebox.showinfo("Success", "Book added")
        self.b_author_id.delete(0, "end")
        self.b_title.delete(0, "end")
        self.b_pages.delete(0, "end")
        self.b_publisher.delete(0, "end")
        self.b_year.delete(0, "end")
        self._refresh_books()

    def _export_author(self, fmt):
        sel = self.authors_tree.selection()
        if not sel:
            messagebox.showwarning("Warning", "Select an author first")
            return
        item = self.authors_tree.item(sel[0])
        author = item["values"]
        if fmt == "json":
            path = filedialog.asksaveasfilename(defaultextension=".json",
                                                filetypes=[("JSON files", "*.json")])
            if path:
                export_author_json(author, path)
                messagebox.showinfo("Success", f"Exported to {path}")
        else:
            path = filedialog.asksaveasfilename(defaultextension=".xml",
                                                filetypes=[("XML files", "*.xml")])
            if path:
                export_author_xml(author, path)
                messagebox.showinfo("Success", f"Exported to {path}")

    def _import_author(self):
        path = filedialog.askopenfilename(filetypes=[("JSON files", "*.json"), ("XML files", "*.xml")])
        if not path:
            return
        try:
            if path.endswith(".json"):
                name, country, years = parse_author_json(path)
            elif path.endswith(".xml"):
                name, country, years = parse_author_xml(path)
            else:
                messagebox.showerror("Error", "Unsupported file format")
                return
            add_author(name, country, years)
            messagebox.showinfo("Success", f"Author '{name}' imported")
            self._refresh_authors()
        except Exception as e:
            messagebox.showerror("Error", f"Failed to parse file: {e}")

    def run(self):
        self.root.mainloop()


if __name__ == "__main__":
    init_db()
    app = LoginWindow()
    app.run()
