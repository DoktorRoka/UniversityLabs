from pymongo import MongoClient
from bson.objectid import ObjectId
import hashlib
import json
import tkinter as tk
from tkinter import ttk, messagebox, filedialog

CLIENT = MongoClient("localhost", 27017)
DB = CLIENT["library_mongo"]


def init_db():
    if "users" not in DB.list_collection_names() or DB.users.count_documents({}) == 0:
        admin_hash = hashlib.sha1("admin".encode()).hexdigest()
        DB.users.insert_one({"login": "admin", "password": admin_hash})

    if DB.authors.count_documents({}) == 0:
        a1 = DB.authors.insert_one({"name": "L.N.Tolstoi", "country": "Russia", "years": "1828-1910"}).inserted_id
        a2 = DB.authors.insert_one({"name": "F.M.Dostoevsky", "country": "Russia", "years": "1821-1881"}).inserted_id
        a3 = DB.authors.insert_one({"name": "J.K.Rowling", "country": "UK", "years": "1965-"}).inserted_id
        DB.books.insert_many([
            {"author_id": a1, "title": "War and Peace", "pages": 1225, "publisher": "First", "year": 1869},
            {"author_id": a1, "title": "Anna Karenina", "pages": 864, "publisher": "Second", "year": 1877},
            {"author_id": a2, "title": "Crime and Punishment", "pages": 671, "publisher": "Third", "year": 1866},
            {"author_id": a3, "title": "Harry Potter", "pages": 309, "publisher": "Bloomsbury", "year": 1997},
        ])


def check_login(login, password):
    user = DB.users.find_one({"login": login})
    if user is None:
        return False
    return user["password"] == hashlib.sha1(password.encode()).hexdigest()


def export_author_json(author, filepath):
    data = {"name": author["name"], "country": author.get("country", ""), "years": author.get("years", "")}
    years = author.get("years", "")
    if years and "-" in years:
        parts = years.split("-")
        try:
            data["years"] = [int(parts[0].strip()), int(parts[1].strip()) if parts[1].strip() else 0]
        except ValueError:
            pass
    with open(filepath, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)


def export_author_xml(author, filepath):
    name = author.get("name", "")
    country = author.get("country", "")
    born = ""
    died = ""
    years = author.get("years", "")
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
        self.root.title("Library (MongoDB) - Login")
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
        self.root.title("Library Information System (MongoDB)")
        self.root.geometry("900x550")

        notebook = ttk.Notebook(self.root)
        notebook.pack(fill="both", expand=True, padx=5, pady=5)

        self.authors_tab = tk.Frame(notebook)
        self.books_tab = tk.Frame(notebook)
        self.add_tab = tk.Frame(notebook)
        self.queries_tab = tk.Frame(notebook)

        notebook.add(self.authors_tab, text="Authors")
        notebook.add(self.books_tab, text="Books")
        notebook.add(self.add_tab, text="Add")
        notebook.add(self.queries_tab, text="Queries")

        self._build_authors_tab()
        self._build_books_tab()
        self._build_add_tab()
        self._build_queries_tab()

    def _build_authors_tab(self):
        self.authors_tree = ttk.Treeview(self.authors_tab,
                                         columns=("id", "name", "country", "years"),
                                         show="headings", height=15)
        self.authors_tree.heading("id", text="ID")
        self.authors_tree.heading("name", text="Name")
        self.authors_tree.heading("country", text="Country")
        self.authors_tree.heading("years", text="Years")
        self.authors_tree.column("id", width=80)
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
        self.books_tree.column("id", width=80)
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

    def _build_queries_tab(self):
        tk.Label(self.queries_tab, text="Predefined MongoDB Queries", font=("Arial", 12, "bold")).pack(pady=5)

        btn_frame = tk.Frame(self.queries_tab)
        btn_frame.pack(fill="x", padx=10, pady=5)

        tk.Button(btn_frame, text="Authors born between X-Y",
                  command=self._query_authors_born_range).pack(fill="x", pady=2)
        tk.Button(btn_frame, text="Books by Russian authors",
                  command=self._query_books_by_russians).pack(fill="x", pady=2)
        tk.Button(btn_frame, text="Books with pages > N",
                  command=self._query_books_pages_gt).pack(fill="x", pady=2)
        tk.Button(btn_frame, text="Authors with more than N books",
                  command=self._query_authors_books_gt).pack(fill="x", pady=2)

        self.query_result = tk.Text(self.queries_tab, height=15, width=80)
        self.query_result.pack(fill="both", expand=True, padx=10, pady=5)

    def _refresh_authors(self):
        for item in self.authors_tree.get_children():
            self.authors_tree.delete(item)
        for a in DB.authors.find():
            self.authors_tree.insert("", "end", values=(str(a["_id"]), a["name"], a.get("country", ""), a.get("years", "")))

    def _refresh_books(self):
        for item in self.books_tree.get_children():
            self.books_tree.delete(item)
        for b in DB.books.find():
            author = DB.authors.find_one({"_id": b["author_id"]})
            author_name = author["name"] if author else ""
            self.books_tree.insert("", "end", values=(
                str(b["_id"]), author_name, b["title"], b.get("pages", ""),
                b.get("publisher", ""), b.get("year", "")))

    def _add_author(self):
        name = self.a_name.get().strip()
        country = self.a_country.get().strip()
        years = self.a_years.get().strip()
        if not name:
            messagebox.showwarning("Warning", "Author name is required")
            return
        DB.authors.insert_one({"name": name, "country": country, "years": years})
        messagebox.showinfo("Success", "Author added")
        self.a_name.delete(0, "end")
        self.a_country.delete(0, "end")
        self.a_years.delete(0, "end")
        self._refresh_authors()

    def _add_book(self):
        author_id_str = self.b_author_id.get().strip()
        try:
            author_id = ObjectId(author_id_str)
        except Exception:
            messagebox.showwarning("Warning", "Invalid Author ID")
            return
        title = self.b_title.get().strip()
        if not title:
            messagebox.showwarning("Warning", "Book title is required")
            return
        pages_str = self.b_pages.get().strip()
        pages = int(pages_str) if pages_str else None
        publisher = self.b_publisher.get().strip()
        year_str = self.b_year.get().strip()
        year = int(year_str) if year_str else None
        doc = {"author_id": author_id, "title": title, "publisher": publisher}
        if pages is not None:
            doc["pages"] = pages
        if year is not None:
            doc["year"] = year
        DB.books.insert_one(doc)
        messagebox.showinfo("Success", "Book added")
        self.b_author_id.delete(0, "end")
        self.b_title.delete(0, "end")
        self.b_pages.delete(0, "end")
        self.b_publisher.delete(0, "end")
        self.b_year.delete(0, "end")
        self._refresh_books()

    def _get_selected_author(self):
        sel = self.authors_tree.selection()
        if not sel:
            messagebox.showwarning("Warning", "Select an author first")
            return None
        item = self.authors_tree.item(sel[0])
        return DB.authors.find_one({"_id": ObjectId(item["values"][0])})

    def _export_author(self, fmt):
        author = self._get_selected_author()
        if author is None:
            return
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
            DB.authors.insert_one({"name": name, "country": country, "years": years})
            messagebox.showinfo("Success", f"Author '{name}' imported")
            self._refresh_authors()
        except Exception as e:
            messagebox.showerror("Error", f"Failed to parse file: {e}")

    def _query_authors_born_range(self):
        X, Y = 1820, 1850
        result_lines = [f"Authors born between {X} and {Y}:"]
        for a in DB.authors.find():
            try:
                born = int(a["years"].split("-")[0].strip())
                if X <= born <= Y:
                    result_lines.append(f"  {a['name']} (born {born})")
            except (ValueError, IndexError, KeyError, AttributeError):
                pass
        if len(result_lines) == 1:
            result_lines.append("  (none found)")
        self.query_result.delete("1.0", "end")
        self.query_result.insert("end", "\n".join(result_lines))

    def _query_books_by_russians(self):
        russian_ids = [a["_id"] for a in DB.authors.find({"country": "Russia"})]
        books = DB.books.find({"author_id": {"$in": russian_ids}})
        result_lines = ["Books by Russian authors:"]
        for b in books:
            author = DB.authors.find_one({"_id": b["author_id"]})
            result_lines.append(f"  {b['title']} by {author['name'] if author else '?'}")
        if len(result_lines) == 1:
            result_lines.append("  (none found)")
        self.query_result.delete("1.0", "end")
        self.query_result.insert("end", "\n".join(result_lines))

    def _query_books_pages_gt(self):
        N = 500
        books = DB.books.find({"pages": {"$gt": N}})
        result_lines = [f"Books with more than {N} pages:"]
        for b in books:
            result_lines.append(f"  {b['title']} ({b['pages']} pages)")
        if len(result_lines) == 1:
            result_lines.append("  (none found)")
        self.query_result.delete("1.0", "end")
        self.query_result.insert("end", "\n".join(result_lines))

    def _query_authors_books_gt(self):
        N = 1
        pipeline = [
            {"$group": {"_id": "$author_id", "count": {"$sum": 1}}},
            {"$match": {"count": {"$gt": N}}}
        ]
        result_lines = [f"Authors with more than {N} book(s):"]
        for doc in DB.books.aggregate(pipeline):
            author = DB.authors.find_one({"_id": doc["_id"]})
            if author:
                result_lines.append(f"  {author['name']} ({doc['count']} books)")
        if len(result_lines) == 1:
            result_lines.append("  (none found)")
        self.query_result.delete("1.0", "end")
        self.query_result.insert("end", "\n".join(result_lines))

    def run(self):
        self.root.mainloop()


if __name__ == "__main__":
    init_db()
    app = LoginWindow()
    app.run()
