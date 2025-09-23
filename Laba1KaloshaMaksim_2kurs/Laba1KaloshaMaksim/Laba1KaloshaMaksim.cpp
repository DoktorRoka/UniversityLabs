#ifdef _MSC_VER // это надо чтобы 3 задание работало, игнорирует переполнение буфера
#define _CRT_SECURE_NO_WARNINGS
#endif

#include <iostream>
#include <cstdio>
#include <clocale>
#include <iomanip>
#include <algorithm>
#include <cstring>


// ВАРИАНТ 5


// задание 1
int processArray(int a[], int n, int out[]) {
    for (int i = 0; i < n; ++i) {
        if (((i + 1) % 2) == 1) {
            int exp = i / 2 + 1;   
            int val = 1;
            for (int e = 0; e < exp; ++e) val *= 3;
            a[i] = val;
        }
        else { 
            int exp = (i + 1) / 2; 
            a[i] = 1 << exp;       
        }
    }

    int count = 0;
    for (int i = 0; i < n; ++i) {
        int v = a[i] >= 0 ? a[i] : -a[i];
        if (v >= 10 && v <= 99) {
            out[count++] = a[i];
        }
    }
    return count;
}

void printArrayC(const char* title, const int a[], int n) {
    std::printf("%s [", title);
    for (int i = 0; i < n; ++i) {
        if (i) std::printf(" ");
        std::printf("%d", a[i]);
    }
    std::printf("]\n");
}
// ------------------------------

// задание 2
void init1D(int* x, int n) {
    for (int i = 0; i < n; ++i) {
        int v = i * i + 1;
        if (((i + 1) % 2) == 0) v = -v;    
        *(x + i) = v;                       
    }
}

void print1D(const int* x, int n, const char* title) {
    if (title && *title) std::cout << title << '\n';
    for (int i = 0; i < n; ++i) {
        std::cout << std::setw(5) << *(x + i);
    }
    std::cout << '\n';
}

int** convert1DTo2D(const int* x, int n, int rows, int cols) {
    int** m = new int* [rows];
    for (int r = 0; r < rows; ++r) {
        *(m + r) = new int[cols];
    }
    int k = 0;
    for (int r = 0; r < rows; ++r) {
        for (int c = 0; c < cols; ++c) {
            *(*(m + r) + c) = *(x + k++);  
        }
    }
    return m;
}

void print2D(int** m, int rows, int cols, const char* title) {
    if (title && *title) std::cout << title << '\n';
    for (int r = 0; r < rows; ++r) {
        for (int c = 0; c < cols; ++c) {
            std::cout << std::setw(5) << *(*(m + r) + c);
        }
        std::cout << '\n';
    }
}

void free2D(int** m, int rows) {
    for (int r = 0; r < rows; ++r) {
        delete[] * (m + r);
    }
    delete[] m;
}
// -----------------------------------------

// задание3
char* my_strcpy(char* dest, const char* src) {
    if (!dest || !src) return dest; 
    char* d = dest;
    while ((*d++ = *src++) != '\0') {
    }
    return dest;
}

// -----------------------------------------
int main()
{
    int my_choice = 0;

    std::setlocale(LC_ALL, "Russian");

    std::cout << "1) Обработка одномерного массива\n";
    std::cout << "2) Преоброзавать одномерный массив в двухмерный\n";
    std::cout << "3) Аналог функции strcpy\n";
    std::cout << "Введите номер: ";
    std::cin >> my_choice;

    switch (my_choice) {
    case 1:
    {
        std::cout << "Задание 1\n";

        const int n = 16;
        int a[n];
        int out[n];

        for (int i = 0; i < n; ++i) a[i] = 1;

        int count = processArray(a, n, out);

        printArrayC("Результирующий массив a:", a, n);

        std::printf("Выходной массив (только двузначные), count = %d: [", count);
        for (int i = 0; i < count; ++i) {
            if (i) std::printf(" ");
            std::printf("%d", out[i]);
        }
        std::printf("]\n");

        break;
    }

    case 2:
    {
        std::cout << "Задание 2\n";

        const int n = 18;
        const int rows = 9, cols = 2;

        int* x = new int[n];
        init1D(x, n);

        print1D(x, n, "Исходный 1D массив:");

        std::sort(x, x + n, [](int a, int b) { return a > b; });
        print1D(x, n, "Отсортированный по убыванию 1D массив:");

        int** m = convert1DTo2D(x, n, rows, cols);
        print2D(m, rows, cols, "Двумерный массив 9x2:");

        delete[] x;
        free2D(m, rows);
        break;
    }
    case 3:
    {
        std::printf("Задание 3\n");

        const char* src1 = "Hello, strcpy!";
        const char* src2 = "";               

        char dest_std1[64] = {};
        char dest_my1[64] = {};

        char dest_std2[64] = {};
        char dest_my2[64] = {};

        std::printf("\n[Тест 1]\n");
        std::printf("Исходная строка: \"%s\"\n", src1);

        // стандартная версия
        std::strcpy(dest_std1, src1);
        std::printf("std::strcpy -> dest_std1: \"%s\"\n", dest_std1);

        // моя версия
        my_strcpy(dest_my1, src1);
        std::printf("my_strcpy   ->  dest_my1: \"%s\"\n", dest_my1);

        std::printf("\n[Тест 2]\n");
        std::printf("Исходная строка: \"%s\"\n", src2);

        std::strcpy(dest_std2, src2);
        std::printf("std::strcpy -> dest_std2: \"%s\"\n", dest_std2);

        my_strcpy(dest_my2, src2);
        std::printf("my_strcpy   ->  dest_my2: \"%s\"\n", dest_my2);

        break;
    }

    default:
        std::cout << "Нет такого пункта в меню.\n";
        break;
    }

    return 0;
}
