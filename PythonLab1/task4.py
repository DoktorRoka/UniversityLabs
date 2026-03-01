def output_words():
    text = input("Введите текст: ")
    tokens = text.split()
    words = []
    for t in tokens:
        cleaned = ''.join(ch for ch in t if ch.isalpha())
        if cleaned:
            words.append(cleaned)
    bigger_than_seven = []
    four_to_seven = []
    other_numbers = []

    for i in words:
        if len(i) > 7:
            bigger_than_seven.append(i)
        elif 4 < len(i) < 7:
            four_to_seven.append(i)
        else:
            other_numbers.append(i)

    print(bigger_than_seven, four_to_seven, other_numbers)


output_words()
