import random

def next_power_of_two(x: int) -> int:
    return 1 << (x - 1).bit_length()

def generate_numbers(seed: int | None = None):

    if seed is not None:
        random.seed(seed)

    n = random.randint(1, 10000)
    arr = [random.randint(-1000, 1000) for _ in range(n)]

    m = next_power_of_two(n)
    zeros_to_add = m - n
    if zeros_to_add:
        arr.extend([0] * zeros_to_add)

    print(f"Сгенерировано n = {n}")
    print(f"Ближайшая степень двойки (не меньше n): {m}")
    print(f"Добавлено нулей: {zeros_to_add}")
    print(f"Итоговая длина массива: {len(arr)}")

    preview = 10
    if len(arr) <= 2 * preview:
        print("Массив целиком:", arr)
    else:
        print("Начало массива:", arr[:preview])
        print("Конец массива:", arr[-preview:])

    return arr

if __name__ == "__main__":
    generate_numbers()