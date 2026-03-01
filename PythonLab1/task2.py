def check_rising():
    my_list = [int(x.strip()) for x in input("Введите числа через ',': ").split(",")]
    # print(my_list)
    result = all(a < b for a, b in zip(my_list, my_list[1:]))
    print(result)


check_rising()
