#define _CRT_SECURE_NO_WARNINGS
#include <filesystem>

#include <algorithm>
#include <cctype>
#include <cstring>
#include <functional>
#include <fstream>
#include <iostream>
#include <iterator>
#include <map>
#include <stdexcept>
#include <string>
#include <vector>

#ifdef _WIN32
#include <cstdio>   // std::remove
#endif

// ===========================
// Task 1: DataManager<T> (Variant 5)
// ===========================

template <typename T>
class DataManager {
    static constexpr std::size_t CAP = 64;
    T data_[CAP]{};
    std::size_t size_ = 0;

    static const char* dumpFile() { return "dump.dat"; }

    void dumpAllToFileAppend() {
        // Работает корректно для int/double/char (тривиально копируемые типы)
        std::ofstream out(dumpFile(), std::ios::binary | std::ios::app);
        if (!out) throw std::runtime_error("Не удалось открыть dump.dat для записи");

        // Пишем размер + сырые байты массива
        unsigned char cnt = static_cast<unsigned char>(size_);
        out.write(reinterpret_cast<const char*>(&cnt), sizeof(cnt));
        out.write(reinterpret_cast<const char*>(data_), static_cast<std::streamsize>(sizeof(T) * size_));
    }

    bool refillFromDumpIfAny() {
        std::ifstream in(dumpFile(), std::ios::binary);
        if (!in) return false;

        // Находим начало последней записи (records: [1 byte count][count*sizeof(T) bytes])
        std::vector<std::streampos> recPos;
        std::vector<unsigned char> recCnt;

        while (true) {
            std::streampos pos = in.tellg();
            unsigned char cnt = 0;
            in.read(reinterpret_cast<char*>(&cnt), sizeof(cnt));
            if (!in) break;
            recPos.push_back(pos);
            recCnt.push_back(cnt);

            // пропускаем данные
            in.seekg(static_cast<std::streamoff>(sizeof(T) * cnt), std::ios::cur);
            if (!in) break;
        }

        if (recPos.empty()) return false;

        const std::streampos lastPos = recPos.back();
        // Читаем последнюю запись
        in.clear();
        in.seekg(lastPos);

        unsigned char cnt = 0;
        in.read(reinterpret_cast<char*>(&cnt), sizeof(cnt));
        if (!in || cnt == 0 || cnt > CAP) return false;

        std::vector<T> buf(cnt);
        in.read(reinterpret_cast<char*>(buf.data()), static_cast<std::streamsize>(sizeof(T) * cnt));
        if (!in) return false;

        // Обрезаем файл: переписываем всё ДО lastPos обратно
        in.clear();
        in.seekg(0, std::ios::beg);

        std::vector<char> prefix(static_cast<std::size_t>(lastPos));
        if (!prefix.empty()) {
            in.read(prefix.data(), static_cast<std::streamsize>(prefix.size()));
            // если не прочиталось — всё равно попробуем trunc ниже
        }
        in.close();

        std::ofstream out(dumpFile(), std::ios::binary | std::ios::trunc);
        if (!out) throw std::runtime_error("Не удалось открыть dump.dat для усечения");
        if (!prefix.empty())
            out.write(prefix.data(), static_cast<std::streamsize>(prefix.size()));

        // Загружаем в контейнер
        size_ = cnt;
        for (std::size_t i = 0; i < size_; ++i) data_[i] = buf[i];
        return true;
    }

public:
    void clear() { size_ = 0; }

    std::size_t size() const { return size_; }
    bool empty() const { return size_ == 0; }

    const T* begin() const { return data_; }
    const T* end() const { return data_ + size_; }

    // Variant 5 push(): в начало, остальные вправо
    void push(const T& elem) {
        if (size_ == CAP) {
            dumpAllToFileAppend();
            clear();
        }
        for (std::size_t i = size_; i > 0; --i) data_[i] = data_[i - 1];
        data_[0] = elem;
        ++size_;
    }

    void push(const T elems[], std::size_t n) {
        for (std::size_t i = 0; i < n; ++i) push(elems[i]);
    }

    // Variant 5 peek(): центральный элемент или 0, если чётно
    T peek() const {
        if (size_ == 0) return T{};
        if (size_ % 2 == 0) return T{};
        return data_[size_ / 2];
    }

    // Variant 5 pop(): средний (при чётном — левый от центра)
    T pop() {
        if (size_ == 0) {
            // не требуется по заданию, но удобно
            refillFromDumpIfAny();
            if (size_ == 0) return T{};
        }

        std::size_t idx = (size_ % 2 == 0) ? (size_ / 2 - 1) : (size_ / 2);
        T val = data_[idx];

        for (std::size_t i = idx + 1; i < size_; ++i) data_[i - 1] = data_[i];
        --size_;

        // если удалили последний — пытаемся подгрузить из файла
        if (size_ == 0) {
            (void)refillFromDumpIfAny();
        }
        return val;
    }
};

// Explicit specialization for char
template <>
class DataManager<char> {
    static constexpr std::size_t CAP = 64;
    char data_[CAP]{};
    std::size_t size_ = 0;

    static const char* dumpFile() { return "dump.dat"; }

    static char normalize(char ch) {
        unsigned char uch = static_cast<unsigned char>(ch);
        if (std::ispunct(uch)) return '_';
        return ch;
    }

    void dumpAllToFileAppend() {
        std::ofstream out(dumpFile(), std::ios::binary | std::ios::app);
        if (!out) throw std::runtime_error("Не удалось открыть dump.dat для записи");
        unsigned char cnt = static_cast<unsigned char>(size_);
        out.write(reinterpret_cast<const char*>(&cnt), sizeof(cnt));
        out.write(reinterpret_cast<const char*>(data_), static_cast<std::streamsize>(sizeof(char) * size_));
    }

    bool refillFromDumpIfAny() {
        std::ifstream in(dumpFile(), std::ios::binary);
        if (!in) return false;

        std::vector<std::streampos> recPos;
        std::vector<unsigned char> recCnt;

        while (true) {
            std::streampos pos = in.tellg();
            unsigned char cnt = 0;
            in.read(reinterpret_cast<char*>(&cnt), sizeof(cnt));
            if (!in) break;
            recPos.push_back(pos);
            recCnt.push_back(cnt);
            in.seekg(static_cast<std::streamoff>(sizeof(char) * cnt), std::ios::cur);
            if (!in) break;
        }
        if (recPos.empty()) return false;

        std::streampos lastPos = recPos.back();
        in.clear();
        in.seekg(lastPos);

        unsigned char cnt = 0;
        in.read(reinterpret_cast<char*>(&cnt), sizeof(cnt));
        if (!in || cnt == 0 || cnt > CAP) return false;

        std::vector<char> buf(cnt);
        in.read(reinterpret_cast<char*>(buf.data()), static_cast<std::streamsize>(sizeof(char) * cnt));
        if (!in) return false;

        // truncate
        in.clear();
        in.seekg(0, std::ios::beg);
        std::vector<char> prefix(static_cast<std::size_t>(lastPos));
        if (!prefix.empty()) in.read(prefix.data(), static_cast<std::streamsize>(prefix.size()));
        in.close();

        std::ofstream out(dumpFile(), std::ios::binary | std::ios::trunc);
        if (!out) throw std::runtime_error("Не удалось открыть dump.dat для усечения");
        if (!prefix.empty())
            out.write(prefix.data(), static_cast<std::streamsize>(prefix.size()));

        size_ = cnt;
        for (std::size_t i = 0; i < size_; ++i) data_[i] = buf[i];
        return true;
    }

public:
    void clear() { size_ = 0; }

    std::size_t size() const { return size_; }
    bool empty() const { return size_ == 0; }

    const char* begin() const { return data_; }
    const char* end() const { return data_ + size_; }

    void push(char ch) {
        ch = normalize(ch);
        if (size_ == CAP) {
            dumpAllToFileAppend();
            clear();
        }
        for (std::size_t i = size_; i > 0; --i) data_[i] = data_[i - 1];
        data_[0] = ch;
        ++size_;
    }

    void push(const char elems[], std::size_t n) {
        for (std::size_t i = 0; i < n; ++i) push(elems[i]);
    }

    char peek() const {
        if (size_ == 0) return '\0';
        if (size_ % 2 == 0) return '\0';
        return data_[size_ / 2];
    }

    char pop() {
        if (size_ == 0) {
            refillFromDumpIfAny();
            if (size_ == 0) return '\0';
        }

        std::size_t idx = (size_ % 2 == 0) ? (size_ / 2 - 1) : (size_ / 2);
        char val = data_[idx];

        for (std::size_t i = idx + 1; i < size_; ++i) data_[i - 1] = data_[i];
        --size_;

        if (size_ == 0) {
            (void)refillFromDumpIfAny();
        }
        return val;
    }

    char popUpper() {
        unsigned char c = static_cast<unsigned char>(pop());
        return static_cast<char>(std::toupper(c));
    }

    char popLower() {
        unsigned char c = static_cast<unsigned char>(pop());
        return static_cast<char>(std::tolower(c));
    }
};

// ===========================
// Task 2: word frequency using std::map
// ===========================

static void runTask2_WordStats(const std::string& filename) {
    namespace fs = std::filesystem;

    std::cout << "Current dir: " << fs::current_path() << "\n";
    std::cout << "Trying file: " << fs::absolute(fs::path(filename)) << "\n";
    std::cout << "Exists? " << (fs::exists(fs::path(filename)) ? "YES" : "NO") << "\n";

    std::ifstream f(filename);
    if (!f) {
        std::cout << "Не удалось открыть файл: " << filename << "\n";
        return;
    }

    std::map<std::string, int> freq;

    const std::size_t MAXLEN = 4096;
    char text[MAXLEN];

    // разделители по заданию: пробел, точка, запятая, тире, двоеточие, !, ;
    // добавим также таб/переводы строк и ? как в примере
    const char* delims = " .,;-:!;\t\r\n?";

    while (f.getline(text, static_cast<std::streamsize>(MAXLEN))) {
        char* token = std::strtok(text, delims);
        while (token != nullptr) {
            std::string word = token;

            // нижний регистр
            std::transform(word.begin(), word.end(), word.begin(),
                [](unsigned char c) { return static_cast<char>(std::tolower(c)); });

            if (word.size() > 3) {
                ++freq[word];
            }
            token = std::strtok(nullptr, delims);
        }
    }

    // переносим в вектор и сортируем по убыванию частоты
    std::vector<std::pair<std::string, int>> v;
    v.reserve(freq.size());
    for (const auto& kv : freq) {
        if (kv.first.size() > 3 && kv.second >= 7) v.push_back(kv);
    }

    std::sort(v.begin(), v.end(),
        [](const auto& a, const auto& b) {
            if (a.second != b.second) return a.second > b.second;
            return a.first < b.first;
        });

    for (const auto& kv : v) {
        std::cout << kv.first << " " << kv.second << "\n";
    }
}

// ===========================
// Task 3/4: Book + sort/find + count_if
// ===========================

class Book {
    std::string name_;
    std::string author_;
    int year_ = 0;

public:
    Book(std::string name, std::string author, int year)
        : name_(std::move(name)), author_(std::move(author)), year_(year) {
    }

    const std::string& getName() const { return name_; }
    const std::string& getAuthor() const { return author_; }
    int getYear() const { return year_; }
};

struct BookSorter {
    bool operator()(const Book* a, const Book* b) const {
        if (a->getAuthor() != b->getAuthor()) return a->getAuthor() < b->getAuthor();
        return a->getName() < b->getName();
    }
};

struct BookFinder {
    int y1, y2;
    BookFinder(int from, int to) : y1(from), y2(to) {}
    bool operator()(const Book* b) const {
        return b->getYear() >= y1 && b->getYear() <= y2;
    }
};

static void runTask3_4_Books() {
    std::vector<Book*> books;
    books.push_back(new Book("Война и мир", "Толстой Л.Н.", 2010));
    books.push_back(new Book("Подросток", "Достоевский Ф.М.", 2004));
    books.push_back(new Book("Обрыв", "Гончаров И.А.", 2010));
    books.push_back(new Book("Анна Каренина", "Толстой Л.Н.", 1999));
    books.push_back(new Book("Обыкновенная история", "Гончаров И.А.", 2011));
    books.push_back(new Book("Утраченные иллюзии", "Бальзак О.", 2009));
    books.push_back(new Book("Оливер Твист", "Диккенс Ч.", 2001));
    books.push_back(new Book("Фауст", "Гёте И.В.", 2010));
    books.push_back(new Book("Лилия долины", "Бальзак О.", 1998));

    std::cout << "\nКниги в алфавитном порядке:\n\n";
    std::sort(books.begin(), books.end(), BookSorter{});
    for (auto* b : books) {
        std::cout << b->getAuthor() << " \"" << b->getName() << "\" (" << b->getYear() << ")\n";
    }

    std::cout << "\nКниги в диапазоне года издания 2005 - 2014:\n\n";
    BookFinder book_finder(2005, 2014);
    auto it = std::find_if(books.begin(), books.end(), book_finder);
    while (it != books.end()) {
        std::cout << (*it)->getAuthor() << " \"" << (*it)->getName() << "\" (" << (*it)->getYear() << ")\n";
        it = std::find_if(std::next(it), books.end(), book_finder);
    }

    // Task 4: count books newer than 2009 using algorithms + functors
    auto newerThan2009 =
        std::bind(std::greater<int>{},
            std::bind(&Book::getYear, std::placeholders::_1),
            2009);

    int count = static_cast<int>(std::count_if(books.begin(), books.end(), newerThan2009));
    std::cout << "\nКоличество книг новее 2009 года: " << count << "\n";

    for (auto* b : books) delete b;
}

// ===========================
// Task 5: Cache<T> + specialization for std::string
// ===========================

template <typename T>
class Cache {
    std::vector<T> data_;

public:
    void put(const T& elem) { data_.push_back(elem); }

    Cache& operator+=(const T& elem) {
        put(elem);
        return *this;
    }

    bool contains(const T& elem) const {
        return std::find(data_.begin(), data_.end(), elem) != data_.end();
    }
};

template <>
class Cache<std::string> {
    std::vector<std::string> data_;

public:
    void put(const std::string& s) {
        if (data_.size() >= 100) {
            throw std::overflow_error("Cache<string>: уже есть 100 строк");
        }
        data_.push_back(s);
    }

    Cache& operator+=(const std::string& s) {
        put(s);
        return *this;
    }

    bool contains(const std::string& s) const {
        if (s.empty()) return false;
        const char first = s[0];

        for (const auto& x : data_) {
            if (!x.empty() && x[0] == first) return true; // совпадение первого символа
        }
        return false;
    }
};

// ===========================
// Demo runners
// ===========================

static void demoTask1_DataManager() {
    // Важно: чтобы типы int/double/char не портили друг другу dump.dat — чистим перед каждым демо
    std::remove("dump.dat");

    std::cout << "\n=== Task 1: DataManager<int> (variant 5) ===\n";
    DataManager<int> mi;

    int a[60] = { 0 };
    mi.push(a, 60);                 // 60 элементов
    for (int i = 1; i <= 10; ++i) { // здесь будет переполнение и dump
        mi.push(i);
    }

    std::cout << "Содержимое (ostream_iterator): ";
    std::copy(mi.begin(), mi.end(), std::ostream_iterator<int>(std::cout, " "));
    std::cout << "\npeek (центр или 0 если чётно): " << mi.peek() << "\n";
    std::cout << "pop (середина/левее центра): " << mi.pop() << "\n";
    std::cout << "После pop: ";
    std::copy(mi.begin(), mi.end(), std::ostream_iterator<int>(std::cout, " "));
    std::cout << "\n";

    std::remove("dump.dat");
    std::cout << "\n=== Task 1: DataManager<double> (variant 5) ===\n";
    DataManager<double> md;
    md.push(1.1);
    md.push(2.2);
    md.push(3.3);
    std::cout << "Данные: ";
    std::copy(md.begin(), md.end(), std::ostream_iterator<double>(std::cout, " "));
    std::cout << "\npeek: " << md.peek() << "\n";

    std::remove("dump.dat");
    std::cout << "\n=== Task 1: DataManager<char> specialization ===\n";
    DataManager<char> mc;
    const char msg[] = { 'h','e','l','l','o','!','?','.' };
    mc.push(msg, sizeof(msg));
    std::cout << "Данные (пунктуация заменена на _): ";
    std::copy(mc.begin(), mc.end(), std::ostream_iterator<char>(std::cout, " "));
    std::cout << "\npopUpper(): " << mc.popUpper() << "\n";
    std::cout << "popLower(): " << mc.popLower() << "\n";
}

static void demoTask5_Cache() {
    std::cout << "\n=== Task 5: Cache<int> and Cache<string> ===\n";

    Cache<int> cache;
    cache.put(1);
    cache.put(2);
    cache.put(3);
    cache += 5;

    Cache<std::string> voc;
    voc.put("OK");
    voc.put("Apple");
    voc.put("Zoo");

    std::cout << "voc.contains(\"Only\") = " << voc.contains("Only") << "\n"; // 'O' совпадает с "OK"
    std::cout << "cache.contains(5) = " << cache.contains(5) << "\n";
}

int main() {
    std::setlocale(LC_ALL, "RUSSIAN");

    while (true) {
        std::cout << "\n=============================\n";
        std::cout << "ЛР5 (вариант 5), без задания 6\n";
        std::cout << "1) Задание 1: DataManager<T>\n";
        std::cout << "2) Задание 2: Частотный анализ слов\n";
        std::cout << "3) Задание 3+4: Книги (sort/find/count_if)\n";
        std::cout << "4) Задание 5: Cache<T>\n";
        std::cout << "0) Выход\n";
        std::cout << "Выбор: ";

        int choice = 0;
        if (!(std::cin >> choice)) break;
        std::cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');

        if (choice == 0) break;

        switch (choice) {
        case 1:
            demoTask1_DataManager();
            break;
        case 2: {
            std::string filename;
            std::cout << "Введите путь к текстовому файлу (например input.txt): ";
            std::getline(std::cin, filename);
            if (filename.empty()) filename = "input.txt";
            runTask2_WordStats(filename);
            break;
        }
        case 3:
            runTask3_4_Books();
            break;
        case 4:
            demoTask5_Cache();
            break;
        default:
            std::cout << "Нет такого пункта.\n";
            break;
        }
    }

    return 0;
}
