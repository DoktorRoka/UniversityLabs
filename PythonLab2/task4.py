import re
import os


def search_phone_numbers():
    filename = input("Введите имя текстового файла (например, text.txt): ")

    # Проверка существования файла
    if not os.path.exists(filename):
        print(f"Ошибка: Файл '{filename}' не найден в текущей директории.")
        return

    # Компилируем регулярное выражение для поиска номеров
    # Разбор шаблона:
    # \(                  - ищем ровно символ '(' (экранируем слешем)
    # \d{3}               - ищем ровно 3 цифры
    # \)                  - ищем ровно символ ')'
    # (?: ... | ... )     - логическое "ИЛИ" (группа без сохранения результата)
    # \d{7}               - ПЕРВЫЙ ВАРИАНТ: ровно 7 цифр подряд
    # \d{3}-\d{2}-\d{2}   - ВТОРОЙ ВАРИАНТ: 3 цифры, дефис, 2 цифры, дефис, 2 цифры
    pattern = re.compile(r'\(\d{3}\)(?:\d{7}|\d{3}-\d{2}-\d{2})')

    print("-" * 40)
    print(f"Результаты поиска в файле '{filename}':")

    matches_found = False

    try:
        with open(filename, 'r', encoding='utf-8') as file:
            for line_num, line in enumerate(file, start=1):

                for match in pattern.finditer(line):
                    matches_found = True
                    pos = match.start()
                    found_text = match.group()

                    print(f"Строка {line_num}, позиция {pos} : найдено '{found_text}'")

    except Exception as e:
        print(f"Произошла ошибка при чтении файла: {e}")

    if not matches_found:
        print("В файле не найдено ни одного подходящего номера телефона.")
    print("-" * 40)


if __name__ == "__main__":
    search_phone_numbers()