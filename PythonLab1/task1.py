def money_format():
    my_number = input("Введите число: ")
    try:
        my_number = my_number.strip().replace(',', '.')
        x = float(my_number)
        if x < 0:
            raise ValueError
        normal_format = int(round(x * 100))
        rub = normal_format // 100
        kop = normal_format % 100
        print(f"{rub} руб. {kop:02d} коп.")
    except ValueError:
        print("Некорректный формат!")


