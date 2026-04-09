import re


class StringFormatter:

    @staticmethod
    def remove_short_words(text, n, sep=' '):
        """Удаление всех слов из строки, длина которых меньше n букв."""
        # Разбиваем строку по разделителю, фильтруем и склеиваем обратно
        words = text.split(sep)
        filtered_words = [word for word in words if len(word) >= n]
        return sep.join(filtered_words)

    @staticmethod
    def replace_digits(text):
        """Замена всех цифр в строке на знак '*'."""
        # Используем регулярное выражение для быстрой замены любых цифр (\d) на *
        return re.sub(r'\d', '*', text)

    @staticmethod
    def insert_spaces(text):
        """Вставка по одному пробелу между всеми символами в строке."""
        # Метод join отлично вставляет указанный символ (пробел) между всеми элементами строки
        return ' '.join(text)

    @staticmethod
    def sort_by_size(text, sep=' '):
        """Сортировка слов по размеру."""
        words = text.split(sep)
        # Сортируем с помощью встроенной функции, передав len как ключ сортировки
        sorted_words = sorted(words, key=len)
        return sep.join(sorted_words)

    @staticmethod
    def sort_lexicographically(text, sep=' '):
        """Сортировка слов в лексикографическом порядке."""
        words = text.split(sep)
        # Обычная сортировка sorted() сортирует строки лексикографически (по алфавиту)
        sorted_words = sorted(words)
        return sep.join(sorted_words)


# --- Код для проверки ---
if __name__ == '__main__':
    # Возьмем строку из скриншотов к 5-му заданию, чтобы проверить правильность логики
    test_str = "your password abcdef12345 is not safe"
    print(f"Исходная строка: '{test_str}'\n")

    # 1. Удаляем слова короче 5 символов
    res1 = StringFormatter.remove_short_words(test_str, 5)
    print(f"1. Без коротких слов (n=5): '{res1}'")
    # Ожидается: 'password abcdef12345'

    # 2. Заменяем цифры на *
    res2 = StringFormatter.replace_digits(res1)
    print(f"2. Без цифр: '{res2}'")
    # Ожидается: 'password abcdef*****'

    # 3. Сортируем лексикографически
    res3 = StringFormatter.sort_lexicographically(res2)
    print(f"3. Лексикографическая сортировка: '{res3}'")
    # Ожидается: 'abcdef***** password'

    # 4. Пробелы между символами (предварительно убрав пробел между словами,
    #    чтобы результат в точности совпал с первым скриншотом из 5-го задания)
    res4 = StringFormatter.insert_spaces(res3.replace(" ", ""))
    print(f"4. Пробелы между символами: '{res4}'")
    # Ожидается: 'a b c d e f * * * * * p a s s w o r d'