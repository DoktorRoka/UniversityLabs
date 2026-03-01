def simulate_atm():
    atm = {
        1000: 10,
        500: 10,
        100: 20,
        50: 20,
        10: 50
    }

    my_amount = int(input("Введите сумму: "))
    result = []

    for bill in sorted(atm.keys(), reverse=True):
        if my_amount <= 0:
            break

        needed = my_amount // bill
        available = atm[bill]

        count = min(needed, available)

        if count > 0:
            result.append(f"{count}*{bill}")
            my_amount -= count * bill

    if my_amount != 0:
        print("Операция не может быть выполнена!")
    else:
        print(" + ".join(result))


simulate_atm()
