import os
import argparse
import subprocess
import random


def get_audio_duration(filepath):
    """
    Получает длительность аудиофайла в секундах с помощью ffprobe (идет в комплекте с ffmpeg).
    """
    cmd = [
        'ffprobe',
        '-v', 'error',
        '-show_entries', 'format=duration',
        '-of', 'default=noprint_wrappers=1:nokey=1',
        filepath
    ]
    try:
        result = subprocess.run(cmd, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True, check=True)
        return float(result.stdout.strip())
    except Exception:
        return 0.0


def create_trackmix(source_dir, destination, count, frame_duration, show_log, use_fade):
    if not os.path.isdir(source_dir):
        print(f"Ошибка: Директория '{source_dir}' не найдена.")
        return

    if not destination:
        destination = os.path.join(source_dir, "mix.mp3")

    mp3_files = [f for f in os.listdir(source_dir) if f.lower().endswith('.mp3')]

    if not mp3_files:
        print(f"В директории '{source_dir}' нет mp3 файлов.")
        return

    if count is not None and count > 0:
        mp3_files = random.sample(mp3_files, min(count, len(mp3_files)))

    if show_log:
        print(f"Найдено файлов для микса: {len(mp3_files)}")
        print(f"Длительность каждого фрагмента: {frame_duration} сек.")

    temp_files = []

    for i, filename in enumerate(mp3_files, start=1):
        if show_log:
            print(f"--- processing file {i}: {filename}")

        filepath = os.path.join(source_dir, filename)
        temp_filepath = os.path.join(source_dir, f"temp_fragment_{i}.mp3")
        temp_files.append(temp_filepath)

        total_duration = get_audio_duration(filepath)

        if total_duration <= frame_duration:
            start_time = 0
            actual_duration = total_duration
        else:
            margin = total_duration * 0.1
            max_start = total_duration - frame_duration - margin
            start_time = random.uniform(margin, max(margin, max_start))
            actual_duration = frame_duration

        # -ss: позиция старта
        # -t: длительность (наш frame)
        ffmpeg_cmd = [
            'ffmpeg', '-y', '-hide_banner', '-loglevel', 'error',
            '-ss', str(start_time),
            '-i', filepath,
            '-t', str(actual_duration)
        ]

        if use_fade:
            fade_duration = 1.0
            fade_filter = f"afade=t=in:ss=0:d={fade_duration},afade=t=out:st={actual_duration - fade_duration}:d={fade_duration}"
            ffmpeg_cmd.extend(['-af', fade_filter])

        ffmpeg_cmd.append(temp_filepath)

        try:
            subprocess.run(ffmpeg_cmd, check=True)
        except subprocess.CalledProcessError as e:
            if show_log:
                print(f"    [Ошибка] Не удалось обработать '{filename}': {e}")

    if show_log:
        print("--- Склейка фрагментов в итоговый микс...")

    list_filepath = os.path.join(source_dir, "concat_list.txt")
    with open(list_filepath, 'w', encoding='utf-8') as f:
        for temp_file in temp_files:
            abs_path = os.path.abspath(temp_file)

            safe_path = abs_path.replace('\\', '/')

            f.write(f"file '{safe_path}'\n")

    concat_cmd = [
        'ffmpeg', '-y', '-hide_banner', '-loglevel', 'error',
        '-f', 'concat',
        '-safe', '0',
        '-i', list_filepath,
        '-c', 'copy',
        destination
    ]

    try:
        subprocess.run(concat_cmd, check=True)
        if show_log:
            print(f"--- Итоговый файл сохранен: {destination}")
            print("--- done!")
    except subprocess.CalledProcessError as e:
        print(f"Ошибка при склейке файлов: {e}")
    finally:
        if os.path.exists(list_filepath):
            os.remove(list_filepath)
        for temp_file in temp_files:
            if os.path.exists(temp_file):
                os.remove(temp_file)


if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Скрипт для создания трек-микса (попурри).")

    parser.add_argument('--source', '-s', required=True, help="Имя рабочей директории с треками")

    parser.add_argument('--destination', '-d', default=None,
                        help="Имя выходного файла (по умолчанию mix.mp3 в директории source)")
    parser.add_argument('--count', '-c', type=int, default=None, help="Количество файлов в нарезке (по умолчанию все)")
    parser.add_argument('--frame', '-f', type=int, default=10,
                        help="Количество секунд на каждый файл (по умолчанию 10)")

    parser.add_argument('--log', '-l', action='store_true', help="Выводить на консоль лог процесса обработки")
    parser.add_argument('--extended', '-e', action='store_true', help="Добавить fade in/fade out для каждого фрагмента")

    args = parser.parse_args()

    create_trackmix(
        source_dir=args.source,
        destination=args.destination,
        count=args.count,
        frame_duration=args.frame,
        show_log=args.log,
        use_fade=args.extended
    )

# пример запуска (НУЖЕН FFMPEG)
# python trackmix.py --source "my_music" --count 3 --frame 15 -l -e
