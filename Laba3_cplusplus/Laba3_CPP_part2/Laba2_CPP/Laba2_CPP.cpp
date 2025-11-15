#include <iostream>
#include <vector>
#include <stdexcept>
#include <cassert>
#include <fstream>
#include <string>
#include <sstream>
#include <iomanip>
#include <algorithm>
#include <cctype>
#include <cmath>
#include <type_traits>
#include <utility>

using namespace std;
namespace util {

    template<typename T>
    string toString(T v) { ostringstream oss; oss << v; return oss.str(); }

    bool parseInt(const string& s, int& out) {
        istringstream iss(s);
        iss >> out;
        return iss && iss.peek() == EOF;
    }
    bool parseLL(const string& s, long long& out) {
        istringstream iss(s);
        iss >> out;
        return iss && iss.peek() == EOF;
    }
}

// ========================= ЗАДАНИЕ 1: Vector / Matrix =========================

template<typename T>
class Vector {

    static_assert(std::is_arithmetic<T>::value, "Vector<T>: T must be arithmetic");
    size_t n_;
    T* data_;
public:
    Vector() : n_(0), data_(nullptr) {}
    explicit Vector(size_t n, T init = T{}) : n_(n), data_(n ? new T[n] : nullptr) {
        for (size_t i = 0;i < n_;++i) data_[i] = init;
    }
    Vector(const Vector& other) : n_(other.n_), data_(n_ ? new T[n_] : nullptr) {
        for (size_t i = 0;i < n_;++i) data_[i] = other.data_[i];
    }
    Vector(Vector&& other) noexcept : n_(other.n_), data_(other.data_) {
        other.n_ = 0; other.data_ = nullptr;
    }
    Vector& operator=(const Vector& other) {
        if (this == &other) return *this;
        Vector tmp(other);
        swap(n_, tmp.n_); swap(data_, tmp.data_);
        return *this;
    }
    Vector& operator=(Vector&& other) noexcept {
        if (this == &other) return *this;
        delete[] data_;
        n_ = other.n_; data_ = other.data_;
        other.n_ = 0; other.data_ = nullptr;
        return *this;
    }
    ~Vector() { delete[] data_; }

    size_t size() const { return n_; }

    T& operator[](size_t i) { assert(i < n_); return data_[i]; }
    const T& operator[](size_t i) const { assert(i < n_); return data_[i]; }

    Vector& operator++() { for (size_t i = 0;i < n_;++i) ++data_[i]; return *this; }
    Vector operator++(int) { Vector tmp(*this); ++(*this); return tmp; }
    Vector& operator--() { for (size_t i = 0;i < n_;++i) --data_[i]; return *this; }
    Vector operator--(int) { Vector tmp(*this); --(*this); return tmp; }

    T sum() const { T s = T{}; for (size_t i = 0;i < n_;++i) s += data_[i]; return s; }
    void scale(T k) { for (size_t i = 0;i < n_;++i) data_[i] *= k; }
    void print(const string& title = "") const {
        if (!title.empty()) cout << title << "\n";
        cout << "[ ";
        for (size_t i = 0;i < n_;++i) cout << data_[i] << (i + 1 < n_ ? " " : "");
        cout << " ]\n";
    }
};

template<typename T>
class Matrix {
    static_assert(std::is_arithmetic<T>::value, "Matrix<T>: T must be arithmetic");
    size_t r_, c_;
    T* data_;
    size_t idx(size_t i, size_t j) const { return i * c_ + j; }
public:
    Matrix() : r_(0), c_(0), data_(nullptr) {}
    Matrix(size_t r, size_t c, T init = T{}) : r_(r), c_(c), data_((r* c) ? new T[r * c] : nullptr) {
        for (size_t i = 0;i < r_ * c_;++i) data_[i] = init;
    }
    Matrix(const Matrix& o) : r_(o.r_), c_(o.c_), data_((r_* c_) ? new T[r_ * c_] : nullptr) {
        for (size_t i = 0;i < r_ * c_;++i) data_[i] = o.data_[i];
    }
    Matrix(Matrix&& o) noexcept : r_(o.r_), c_(o.c_), data_(o.data_) { o.r_ = o.c_ = 0; o.data_ = nullptr; }
    Matrix& operator=(const Matrix& o) {
        if (this == &o) return *this;
        Matrix tmp(o);
        swap(r_, tmp.r_); swap(c_, tmp.c_); swap(data_, tmp.data_);
        return *this;
    }
    Matrix& operator=(Matrix&& o) noexcept {
        if (this == &o) return *this;
        delete[] data_;
        r_ = o.r_; c_ = o.c_; data_ = o.data_;
        o.r_ = o.c_ = 0; o.data_ = nullptr;
        return *this;
    }
    ~Matrix() { delete[] data_; }

    size_t rows() const { return r_; }
    size_t cols() const { return c_; }

    T at(int i, int j) const {
        if (i < 0 || j < 0 || (size_t)i >= r_ || (size_t)j >= c_) throw out_of_range("Matrix::at");
        return data_[idx((size_t)i, (size_t)j)];
    }
    void setAt(int i, int j, T val) {
        if (i < 0 || j < 0 || (size_t)i >= r_ || (size_t)j >= c_) throw out_of_range("Matrix::setAt");
        data_[idx((size_t)i, (size_t)j)] = val;
    }

    Matrix& operator++() { for (size_t i = 0;i < r_ * c_;++i) ++data_[i]; return *this; }
    Matrix operator++(int) { Matrix tmp(*this); ++(*this); return tmp; }
    Matrix& operator--() { for (size_t i = 0;i < r_ * c_;++i) --data_[i]; return *this; }
    Matrix operator--(int) { Matrix tmp(*this); --(*this); return tmp; }

    void print(const string& title = "") const {
        if (!title.empty()) cout << title << "\n";
        for (size_t i = 0;i < r_;++i) {
            cout << "[ ";
            for (size_t j = 0;j < c_;++j) cout << data_[idx(i, j)] << (j + 1 < c_ ? " " : "");
            cout << " ]\n";
        }
    }
    Matrix transposed() const {
        Matrix t(c_, r_);
        for (size_t i = 0;i < r_;++i)
            for (size_t j = 0;j < c_;++j)
                t.setAt((int)j, (int)i, data_[idx(i, j)]);
        return t;
    }
    Matrix operator+(const Matrix& b) const {
        if (r_ != b.r_ || c_ != b.c_) throw runtime_error("Matrix::operator+ size mismatch");
        Matrix s(r_, c_);
        for (size_t i = 0;i < r_ * c_;++i) s.data_[i] = data_[i] + b.data_[i];
        return s;
    }
    Matrix operator*(const Matrix& b) const {
        if (c_ != b.r_) throw runtime_error("Matrix::operator* size mismatch");
        Matrix m(r_, b.c_, T{});
        for (size_t i = 0;i < r_;++i)
            for (size_t k = 0;k < c_;++k) {
                T aik = data_[idx(i, k)];
                for (size_t j = 0;j < b.c_;++j)
                    m.setAt((int)i, (int)j, m.at((int)i, (int)j) + aik * b.data_[b.idx(k, j)]);
            }
        return m;
    }
};

// ========================= ЗАДАНИЕ 2: Fraction =========================

class Fraction {
    int num_;
    int den_;
    static int s_alive_;
    static int s_abs(int x) { return x < 0 ? -x : x; }
public:
    Fraction() : num_(0), den_(1) { ++s_alive_; }
    Fraction(int n, int d) {
        if (d == 0) throw invalid_argument("denominator == 0");
        if (d < 0) { n = -n; d = -d; }
        num_ = n; den_ = d; reduce();
        ++s_alive_;
    }
    Fraction(const Fraction& f) : num_(f.num_), den_(f.den_) { ++s_alive_; }
    Fraction(Fraction&& f) noexcept : num_(f.num_), den_(f.den_) { ++s_alive_; }
    ~Fraction() { --s_alive_; }

    static int alive() { return s_alive_; }

    static int gcd(int a, int b) {
        a = s_abs(a); b = s_abs(b);
        if (a == 0) return b ? b : 1;
        if (b == 0) return a;
        while (b) { int t = a % b; a = b; b = t; }
        return a;
    }

    void reduce() {
        int g = gcd(num_, den_);
        if (g > 1) { num_ /= g; den_ /= g; }
        if (den_ < 0) { den_ = -den_; num_ = -num_; }
    }

    Fraction operator+(const Fraction& o) const {
        return Fraction(num_ * o.den_ + o.num_ * den_, den_ * o.den_);
    }
    Fraction operator-(const Fraction& o) const {
        return Fraction(num_ * o.den_ - o.num_ * den_, den_ * o.den_);
    }
    Fraction operator*(const Fraction& o) const {
        return Fraction(num_ * o.num_, den_ * o.den_);
    }
    Fraction operator/(const Fraction& o) const {
        if (o.num_ == 0) throw invalid_argument("division by zero fraction");
        return Fraction(num_ * o.den_, den_ * o.num_);
    }

    string str() const {
        return util::toString(num_) + "/" + util::toString(den_);
    }

    static void printAsFraction(const char* decimal_fraction) {
        if (!decimal_fraction) { cout << "0/1\n"; return; }
        string s(decimal_fraction);
        s.erase(remove_if(s.begin(), s.end(),
            [](unsigned char ch) { return std::isspace(ch); }), s.end());
        if (s.empty()) { cout << "0/1\n"; return; }

        bool neg = false;
        if (s[0] == '+' || s[0] == '-') { neg = (s[0] == '-'); s = s.substr(1); }
        size_t pos = s.find('.');
        if (pos == string::npos) {
            int n = 0; if (!s.empty()) util::parseInt(s, n);
            if (neg) n = -n;
            Fraction f(n, 1);
            cout << f.str() << "\n";
            return;
        }
        string intPart = s.substr(0, pos);
        string fracPart = s.substr(pos + 1);
        while (!fracPart.empty() && fracPart.back() == '0') fracPart.pop_back();
        if (fracPart.empty()) {
            int n = 0; if (!intPart.empty()) util::parseInt(intPart, n);
            if (neg) n = -n;
            Fraction f(n, 1);
            cout << f.str() << "\n";
            return;
        }
        long long i = 0, p = 0;
        if (!intPart.empty()) util::parseLL(intPart, i);
        if (!fracPart.empty()) util::parseLL(fracPart, p);
        long long q = 1;
        for (size_t k = 0;k < fracPart.size();++k) q *= 10;
        long long n = i * q + p;
        if (neg) n = -n;
        Fraction f((int)n, (int)q);
        cout << f.str() << "\n";
    }

    static void printAsFraction(double x) {
        ostringstream oss;
        oss.setf(std::ios::fixed);
        oss << setprecision(12) << x;
        string s = oss.str();
        if (s.find('.') != string::npos) {
            while (!s.empty() && s.back() == '0') s.pop_back();
            if (!s.empty() && s.back() == '.') s.pop_back();
        }
        printAsFraction(s.c_str());
    }
};
int Fraction::s_alive_ = 0;

// ========================= ЗАДАНИЕ 3: Вариант 5 — КОНДИЦИОНЕР =========================
// Интерфейс управления устройством
class IControllable {
public:
    virtual void control(int temperature) = 0;   // отрегулировать по заданной температуре
    virtual ~IControllable() = default;
};

// Базовый класс "БЫТОВОЕ УСТРОЙСТВО"
class HouseholdDevice {
protected:
    string brand_;
    string model_;
    double weight_ = 0.0;   // кг
    double temperature_ = 20.0; // текущая целевая температура
    int    mode_ = 0;     // 0 – выключено, >0 – разные режимы
    int    year_ = 2000;
    double power_ = 0.0;   // мощность (кВт, произвольно)

public:
    HouseholdDevice() = default;

    HouseholdDevice(string brand, string model,
        double weight, double temperature,
        int mode, int year, double power)
        : brand_(std::move(brand)),
        model_(std::move(model)),
        weight_(weight),
        temperature_(temperature),
        mode_(mode),
        year_(year),
        power_(power) {
    }

    virtual ~HouseholdDevice() = default;

    // простые геттеры
    const string& brand()  const { return brand_; }
    const string& model()  const { return model_; }
    double        weight() const { return weight_; }
    double        temperature() const { return temperature_; }
    int           mode()   const { return mode_; }
    int           year()   const { return year_; }
    double        power()  const { return power_; }

    // и сеттеры
    void setBrand(const string& b) { brand_ = b; }
    void setModel(const string& m) { model_ = m; }
    void setWeight(double w) { weight_ = w; }
    void setTemperature(double t) { temperature_ = t; }
    void setMode(int m) { mode_ = m; }
    void setYear(int y) { year_ = y; }
    void setPower(double p) { power_ = p; }

    // виртуальный метод для полиморфного вывода
    virtual void printInfo() const {
        cout << brand_ << " " << model_
            << " (" << year_ << ")"
            << ", power=" << power_ << " kW"
            << ", weight=" << weight_ << " kg"
            << ", mode=" << mode_
            << ", temp=" << temperature_ << "°C";
    }
};

// КОНДИЦИОНЕР
class AirConditioner : public HouseholdDevice, public IControllable {
    // просто чтобы показать "творческое" поле — среднее изменение температуры
    vector<double> temp_changes_;

public:
    AirConditioner() = default;

    AirConditioner(string brand, string model,
        double weight, double temperature,
        int year, double power)
        : HouseholdDevice(std::move(brand), std::move(model),
            weight, temperature,
            /*mode*/0, year, power) {
    }

    void setTemperature(double t) {
        double delta = std::fabs(temperature_ - t);
        temperature_ = t;
        temp_changes_.push_back(delta);
    }

    double avgTempChange() const {
        if (temp_changes_.empty()) return 0.0;
        double s = 0;
        for (double d : temp_changes_) s += d;
        return s / temp_changes_.size();
    }

    // Реализация IControllable для КОНДИЦИОНЕРА
    void control(int temperature) override {
        cout << "AirConditioner '" << brand_ << " " << model_
            << "': control(" << temperature << ")\n";

        if (temperature < 10) {
            cout << "  Температура " << temperature
                << "°C слишком низкая, кондиционер выключен.\n";
            mode_ = 0;                // Off
            setTemperature(temperature);
            return;
        }

        // присвоить режим в зависимости от температуры
        if (temperature <= 18)
            mode_ = 1;                // сильное охлаждение
        else if (temperature <= 24)
            mode_ = 2;                // комфорт
        else
            mode_ = 3;                // лёгкое охлаждение/вентиляция

        setTemperature(temperature);

        cout << "  Установлен режим #" << mode_
            << ", температура " << temperature_ << "°C\n";
    }

    void printInfo() const override {
        cout << "AirConditioner: ";
        HouseholdDevice::printInfo();
        cout << ", avgΔT=" << fixed << setprecision(2)
            << avgTempChange() << "\n";
    }
};

// ОБОГРЕВАТЕЛЬ
class Heater : public HouseholdDevice, public IControllable {
public:
    Heater() = default;

    Heater(string brand, string model,
        double weight, double temperature,
        int year, double power)
        : HouseholdDevice(std::move(brand), std::move(model),
            weight, temperature,
            /*mode*/0, year, power) {
    }

    // Реализация IControllable для ОБОГРЕВАТЕЛЯ
    void control(int temperature) override {
        cout << "Heater '" << brand_ << " " << model_
            << "': control(" << temperature << ")\n";

        if (temperature > 45) {
            cout << "  Температура " << temperature
                << "°C слишком высокая, обогреватель выключен.\n";
            mode_ = 0;                // Off
            temperature_ = temperature;
            return;
        }

        temperature_ = temperature;

        if (temperature < 25)
            mode_ = 1;                // слабый нагрев
        else if (temperature < 35)
            mode_ = 2;                // средний
        else
            mode_ = 3;                // сильный нагрев

        cout << "  Установлен режим #" << mode_
            << ", температура " << temperature_ << "°C\n";
    }

    void printInfo() const override {
        cout << "Heater: ";
        HouseholdDevice::printInfo();
        cout << "\n";
    }
};

// ========================= MAIN =========================
int main() {
    setlocale(LC_ALL, "Russian");

    cout << "===== Задание 1: Vector / Matrix =====\n";
    Vector<int> vi(5, 1);
    vi[2] = 10;
    vi.print("Vector before:");
    ++vi;
    vi.print("Vector after ++:");
    vi--;
    vi.print("Vector after -- (post):");
    cout << "Sum = " << vi.sum() << "\n\n";

    Matrix<double> A(2, 3, 1.5);
    A.setAt(0, 1, 4.2);
    A.setAt(1, 2, -3.0);
    A.print("Matrix A:");
    (A++).print("Matrix A (A++ returned copy):");
    A.print("Matrix A after A++:");
    auto AT = A.transposed();
    AT.print("A^T:");

    Matrix<double> B(3, 2, 2.0);
    auto C = A * B;
    C.print("A * B:");

    cout << "\n===== Задание 2: Fraction =====\n";
    Fraction f1(1, 3), f2(2, 5);
    cout << "Alive fractions: " << Fraction::alive() << "\n";
    auto fsum = f1 + f2;
    auto fdif = f1 - f2;
    auto fmul = f1 * f2;
    auto fdiv = f1 / f2;
    cout << "f1 = " << f1.str() << ", f2 = " << f2.str() << "\n";
    cout << "f1+f2 = " << fsum.str() << "\n";
    cout << "f1-f2 = " << fdif.str() << "\n";
    cout << "f1*f2 = " << fmul.str() << "\n";
    cout << "f1/f2 = " << fdiv.str() << "\n";
    cout << "printAsFraction(0.25) = "; Fraction::printAsFraction(0.25);
    cout << "printAsFraction(\"0.43\") = "; Fraction::printAsFraction("0.43");
    cout << "Alive fractions now: " << Fraction::alive() << "\n";

    cout << "\n===== Задание 3 (Вариант 5): КОНДИЦИОНЕР =====\n";

    // массив указателей на базовый класс
    HouseholdDevice* devices[3];

    // 2 кондиционера и 1 обогреватель
    devices[0] = new AirConditioner("Midea", "Breeze10", 9.8, 24.0, 2022, 2.5);
    devices[1] = new AirConditioner("Daikin", "FTXM35", 10.5, 23.0, 2021, 3.1);
    devices[2] = new Heater("Electrolux", "HeatPro", 7.2, 20.0, 2020, 2.0);

    cout << "Initial state:\n";
    for (auto* d : devices) {
        d->printInfo();            // виртуальный метод → полиморфизм по базовому классу
    }

    // Температуры, которые мы "задаём" устройствам
    int temps[] = { 18, 8, 50 };

    cout << "\nApplying control() polymorphically via IControllable* (RTTI + интерфейс):\n";
    for (int i = 0; i < 3; ++i) {
        // RTTI: пытаемся привести HouseholdDevice* к интерфейсу IControllable
        if (auto* ctrl = dynamic_cast<IControllable*>(devices[i])) {
            ctrl->control(temps[i]);   // один и тот же интерфейсный метод → разное поведение
        }
    }

    cout << "\nAfter control:\n";
    for (auto* d : devices) {
        d->printInfo();
    }

    // Ещё пример RTTI: выясняем конкретный тип объекта
    cout << "\nRTTI type check:\n";
    for (auto* d : devices) {
        if (dynamic_cast<AirConditioner*>(d)) {
            cout << "  Это кондиционер.\n";
        }
        else if (dynamic_cast<Heater*>(d)) {
            cout << "  Это обогреватель.\n";
        }
    }

    for (auto* d : devices) {
        delete d;
    }

    return 0;
}
