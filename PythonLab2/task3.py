import os
import re


def rename_music_files(music_dir, tracklist_file):
    print(f"Чтение списка треков из: {tracklist_file}")

    # Словарь для хранения данных о треках. Ключ - название в нижнем регистре, значение - кортеж (номер, правильное_название)
    track_data = {}

    # Регулярное выражение для разбора строки "01. Freefall [6:12]"
    # ^(\d+)    - захватывает цифры в начале (номер)
    # \.\s+     - пропускает точку и пробел(ы)
    # (.+?)     - лениво захватывает название песни
    # \s+\[.+\] - пропускает пробел и скобки с длительностью [6:12] в конце
    pattern = re.compile(r'^(\d+)\.\s+(.+?)\s+\[.+?\]$')

    try:
        with open(tracklist_file, 'r', encoding='utf-8') as file:
            for line in file:
                line = line.strip()
                if not line:
                    continue

                match = pattern.match(line)
                if match:
                    track_num = match.group(1)  # "01"
                    title = match.group(2)  # "Freefall"

                    track_data[title.lower()] = (track_num, title)
                else:
                    print(f"Строка не распознана: {line}")
    except FileNotFoundError:
        print(f"Ошибка: Файл со списком '{tracklist_file}' не найден.")
        return

    print("\nПроверка файлов в директории...")

    if not os.path.exists(music_dir):
        print(f"Ошибка: Директория '{music_dir}' не найдена.")
        return

    renamed_count = 0
    for filename in os.listdir(music_dir):
        old_filepath = os.path.join(music_dir, filename)

        if not os.path.isfile(old_filepath):
            continue

        name, ext = os.path.splitext(filename)

        normalized_name = name.strip().lower()

        if normalized_name in track_data:
            track_num, proper_title = track_data[normalized_name]

            new_filename = f"{track_num}. {proper_title}{ext}"
            new_filepath = os.path.join(music_dir, new_filename)

            if old_filepath != new_filepath:
                try:
                    os.rename(old_filepath, new_filepath)
                    print(f"Успешно: '{filename}' -> '{new_filename}'")
                    renamed_count += 1
                except Exception as e:
                    print(f"Ошибка при переименовании '{filename}': {e}")
        else:
            if ext.lower() in ['.mp3', '.flac', '.wav', '.ogg', '.m4a']:
                print(f"Пропущен (нет в списке): '{filename}'")

    print(f"\nГотово! Переименовано файлов: {renamed_count}")


# --- Блок запуска ---
if __name__ == "__main__":
    # Укажите путь к папке с музыкой
    MUSIC_DIRECTORY = "my_music"

    # Укажите путь к текстовому файлу со списком
    TRACKLIST_TXT = "tracklist.txt"

    rename_music_files(MUSIC_DIRECTORY, TRACKLIST_TXT)