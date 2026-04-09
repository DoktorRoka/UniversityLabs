# Базовый класс, выступающий в роли интерфейса
class Taggable:
    def tag(self):
        raise NotImplementedError("Метод tag() должен быть реализован в классе-наследнике")


# Класс "Книга", реализующий "интерфейс" Taggable
class Book(Taggable):
    def __init__(self, author, title):
        # Проверка на пустое название, как требуется в задании
        if not title:
            raise ValueError("Название книги не может быть пустым")

        self.author = author
        self.title = title
        self.code = None  # Код будет назначен библиотекой при добавлении

    def tag(self):
        # Разбиваем строку по пробелам и выбираем слова, начинающиеся с заглавной буквы
        return [word for word in self.title.split() if word and word[0].isupper()]

    def __str__(self):
        # Форматируем имя автора (из "Leo Tolstoi" в "L.Tolstoi")
        parts = self.author.split()
        if len(parts) >= 2:
            author_formatted = f"{parts[0][0]}.{parts[1]}"
        else:
            author_formatted = self.author

        return f"[{self.code}] {author_formatted} '{self.title}'"


# Класс "Библиотека"
class Library:
    # Статический член класса для автоматического назначения кода книгам
    _book_counter = 1

    def __init__(self, number, address):
        self.number = number
        self.address = address
        # Инициализируем пустой список книг
        self.books = []

    # Перегрузка оператора +=
    def __iadd__(self, book):
        if isinstance(book, Book):
            # Присваиваем книге текущее значение счетчика
            book.code = Library._book_counter
            # Увеличиваем счетчик для следующей книги
            Library._book_counter += 1
            # Добавляем книгу в список
            self.books.append(book)
        return self

    # Делаем объект библиотеки итерируемым для цикла for
    def __iter__(self):
        return iter(self.books)


# --- Код для проверки из задания ---
if __name__ == '__main__':
    lib = Library(1, '51 Some str., NY')
    lib += Book('Leo Tolstoi', 'War and Peace')
    lib += Book('Charles Dickens', 'David Copperfield')

    for book in lib:
        # вывод в виде: [1] L.Tolstoi ‘War and Peace’
        print(book)
        # вывод в виде:[‘War’, ‘Peace’]
        print(book.tag())
        print("-" * 20)