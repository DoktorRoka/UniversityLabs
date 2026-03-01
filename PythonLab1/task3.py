def steal_money():
    my_input = input("Введите номер карты (16 цифр): ")
    digits = ''.join(ch for ch in my_input if ch.isdigit())
    masked = f"{digits[:4]} **** **** {digits[-4:]}"

    print(masked)


steal_money()
