import os
import hashlib
from collections import defaultdict


def get_file_md5(file_path, chunk_size=8192):
    hasher = hashlib.md5()
    try:
        with open(file_path, 'rb') as f:
            while chunk := f.read(chunk_size):
                hasher.update(chunk)
        return hasher.hexdigest()
    except (PermissionError, OSError):
        return None


def find_duplicate_files(target_directory):
    print(f"Сканирование директории: {target_directory} ...")

    # Ключ - размер в байтах, значение - список путей к файлам
    files_by_size = defaultdict(list)

    for root, dirs, files in os.walk(target_directory):
        for filename in files:
            file_path = os.path.join(root, filename)
            try:
                # Получаем размер файла
                size = os.path.getsize(file_path)
                files_by_size[size].append(file_path)
            except OSError:
                continue

    # Ключ - MD5 хеш, значение - список путей к файлам-дубликатам
    files_by_hash = defaultdict(list)

    for size, paths in files_by_size.items():
        # Если файл такого размера только один, это не дубликат
        if len(paths) > 1:
            for file_path in paths:
                file_hash = get_file_md5(file_path)
                if file_hash:
                    files_by_hash[file_hash].append(file_path)

    duplicates = {md5: paths for md5, paths in files_by_hash.items() if len(paths) > 1}
    return duplicates


if __name__ == "__main__":
    DIRECTORY_TO_SCAN = "test_dir/"

    duplicates = find_duplicate_files(DIRECTORY_TO_SCAN)

    if not duplicates:
        print("Дубликатов не найдено.")
    else:
        print(f"\nНайдено групп дубликатов: {len(duplicates)}\n")

        group_number = 1
        for md5_hash, file_paths in duplicates.items():
            print(f"--- Группа {group_number} (MD5: {md5_hash}) ---")
            for path in file_paths:
                print(f"  {path}")
            print()  # Пустая строка для разделения групп
            group_number += 1