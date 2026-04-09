class Fraction(object):
    def __init__(self, num, den):
        self.__num = num
        self.__den = den
        self.reduce()

    def __str__(self):
        return "%d/%d" % (self.__num, self.__den)

    def reduce(self):
        g = Fraction.gcd(self.__num, self.__den)
        # Используем //= вместо /= для совместимости с Python 3 (целочисленное деление)
        self.__num //= g
        self.__den //= g

    @staticmethod
    def gcd(n, m):
        if m == 0:
            return n
        else:
            return Fraction.gcd(m, n % m)

    # --- Добавленные магические методы ---

    def __neg__(self):
        # Возвращаем новую дробь с отрицательным числителем
        return Fraction(-self.__num, self.__den)

    def __invert__(self):
        # Меняем числитель и знаменатель местами
        return Fraction(self.__den, self.__num)

    def __pow__(self, power):
        # Возводим в степень отдельно числитель и знаменатель
        return Fraction(self.__num ** power, self.__den ** power)

    def __float__(self):
        # Возвращаем результат обычного деления
        return self.__num / self.__den

    def __int__(self):
        # Возвращаем целую часть от деления
        return self.__num // self.__den


# --- Код для проверки из задания ---
if __name__ == '__main__':
    frac = Fraction(7, 2)
    print(-frac)        # выводит -7/2
    print(~frac)        # выводит 2/7
    print(frac**2)      # выводит 49/4
    print(float(frac))  # выводит 3.5
    print(int(frac))    # выводит 3