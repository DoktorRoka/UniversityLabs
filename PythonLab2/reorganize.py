import os
import shutil
import argparse
from datetime import datetime, timedelta


def create_dir_if_needed(path):
    if not os.path.exists(path):
        os.makedirs(path)
        print(f"Создана директория: {path}")


def reorganize_files(source_dir, days, size_limit):
    if not os.path.isdir(source_dir):
        print(f"Ошибка: Директория '{source_dir}' не найдена или это не папка.")
        return

    archive_dir = os.path.join(source_dir, 'Archive')
    small_dir = os.path.join(source_dir, 'Small')

    now = datetime.now()

    files_to_archive = []
    files_to_small = []

    #Сканируем директорию и распределяем файлы по спискам
    for filename in os.listdir(source_dir):
        file_path = os.path.join(source_dir, filename)

        if not os.path.isfile(file_path):
            continue

        try:
            file_size = os.path.getsize(file_path)

            mtime_timestamp = os.path.getmtime(file_path)
            file_mtime = datetime.fromtimestamp(mtime_timestamp)

            age_delta = now - file_mtime

            if age_delta.days > days:
                files_to_archive.append(file_path)
                continue

            if file_size < size_limit:
                files_to_small.append(file_path)

        except Exception as e:
            print(f"Ошибка при обработке файла '{filename}': {e}")


    if files_to_archive:
        create_dir_if_needed(archive_dir)
        print(f"\nПеремещение старых файлов (старше {days} дней) в Archive:")
        for file_path in files_to_archive:
            try:
                shutil.move(file_path, archive_dir)
                print(f"  -> {os.path.basename(file_path)}")
            except Exception as e:
                print(f"  [Ошибка] Не удалось переместить {os.path.basename(file_path)}: {e}")
    else:
        print(f"Файлов старше {days} дней не найдено.")

    if files_to_small:
        create_dir_if_needed(small_dir)
        print(f"\nПеремещение маленьких файлов (меньше {size_limit} байт) в Small:")
        for file_path in files_to_small:
            try:
                shutil.move(file_path, small_dir)
                print(f"  -> {os.path.basename(file_path)}")
            except Exception as e:
                print(f"  [Ошибка] Не удалось переместить {os.path.basename(file_path)}: {e}")
    else:
        print(f"Файлов размером менее {size_limit} байт не найдено.")


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Скрипт для реорганизации файлов в директории.")

    parser.add_argument('--source', required=True, help='Путь к исходной директории')
    parser.add_argument('--days', type=int, required=True, help='Количество дней (возраст файлов для архивации)')
    parser.add_argument('--size', type=int, required=True, help='Максимальный размер в байтах для маленьких файлов')

    args = parser.parse_args()

    reorganize_files(args.source, args.days, args.size)

# комманда для теста
# python reorganize.py --source "TestDir" --days 2 --size 4096


