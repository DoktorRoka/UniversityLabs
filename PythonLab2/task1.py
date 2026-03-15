import sys
from collections import Counter


def analyze_letter_frequency(file_path):
    try:
        with open(file_path, 'r', encoding='utf-8') as file:
            text = file.read()

        text = text.lower()

        letters = [char for char in text if char.isalpha()]

        if not letters:
            print("В файле нет буквенных символов.")
            return

        total_letters = len(letters)

        letter_counts = Counter(letters)

        print(f"Анализ файла: {file_path}")
        print(f"Всего букв найдено: {total_letters}")
        print("-" * 30)
        print(f"{'Буква':<10} | {'Количество':<12} | {'Частота (%)'}")
        print("-" * 30)

        for letter, count in letter_counts.most_common():
            percentage = (count / total_letters) * 100
            print(f"{letter:<10} | {count:<12} | {percentage:.2f}%")

        print("\n")

    except FileNotFoundError:
        print(f"Ошибка: Файл '{file_path}' не найден.")
    except Exception as e:
        print(f"Произошла ошибка: {e}")


if __name__ == "__main__":
    files_to_check = ['english_text.txt', 'russian_text.txt']

    for file in files_to_check:
        analyze_letter_frequency(file)