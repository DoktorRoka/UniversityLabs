def check_password():
    password = input("Введите пароль: ")

    if len(password) < 8:
        print("Пароль ненадёжный (меньше 8 символов)")
        return

    has_digit = any(ch.isdigit() for ch in password)
    has_upper = any(ch.isupper() for ch in password)
    has_lower = any(ch.islower() for ch in password)
    has_symbol = any(not ch.isalnum() for ch in password)

    if has_digit and has_upper and has_lower and has_symbol:
        print("Пароль надёжный")
    else:
        print("Пароль ненадёжный")

check_password()