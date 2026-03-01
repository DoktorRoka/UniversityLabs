from collections import Counter

def print_all_symbols():
    s = input("Введите текст: ")
    counts = Counter(s)
    unique = [ch for ch in s if counts[ch] == 1]
    print("Символы, встречающиеся ровно один раз:", unique)
    print("Сформированная строка из этих символов:", ''.join(unique))

print_all_symbols()