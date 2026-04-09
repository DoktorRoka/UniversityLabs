import sys
import re
from PyQt6.QtWidgets import (QApplication, QWidget, QVBoxLayout, QHBoxLayout,
                             QLabel, QLineEdit, QCheckBox, QSpinBox,
                             QRadioButton, QPushButton, QButtonGroup)


# Класс из Задания №4
class StringFormatter:
    @staticmethod
    def remove_short_words(text, n, sep=' '):
        words = text.split(sep)
        return sep.join([w for w in words if len(w) >= n])

    @staticmethod
    def replace_digits(text):
        return re.sub(r'\d', '*', text)

    @staticmethod
    def insert_spaces(text):
        # Чтобы результат в точности совпадал со скриншотом из задания,
        # сначала убираем старые пробелы, а затем вставляем новые между всеми символами
        return ' '.join(text.replace(' ', ''))

    @staticmethod
    def sort_by_size(text, sep=' '):
        words = text.split(sep)
        return sep.join(sorted(words, key=len))

    @staticmethod
    def sort_lexicographically(text, sep=' '):
        words = text.split(sep)
        return sep.join(sorted(words))


# Класс Графического интерфейса
class FormatterApp(QWidget):
    def __init__(self):
        super().__init__()
        self.init_ui()

    def init_ui(self):
        self.setWindowTitle("StringFormatter Demo")
        self.resize(450, 300)

        main_layout = QVBoxLayout()

        # --- 1. Поле ввода исходной строки ---
        input_layout = QHBoxLayout()
        self.lbl_input = QLabel("Строка:")
        self.le_input = QLineEdit("your password abcdef12345 is not safe")
        input_layout.addWidget(self.lbl_input)
        input_layout.addWidget(self.le_input)
        main_layout.addLayout(input_layout)

        # --- 2. Чекбокс: Удаление коротких слов ---
        rm_layout = QHBoxLayout()
        self.cb_remove = QCheckBox("Удалить слова размером меньше")
        self.spin_n = QSpinBox()
        self.spin_n.setValue(5)
        self.lbl_letters = QLabel("букв")

        rm_layout.addWidget(self.cb_remove)
        rm_layout.addWidget(self.spin_n)
        rm_layout.addWidget(self.lbl_letters)
        rm_layout.addStretch()  # Прижимает элементы влево
        main_layout.addLayout(rm_layout)

        # --- 3. Чекбокс: Замена цифр ---
        self.cb_replace = QCheckBox("Заменить все цифры на *")
        main_layout.addWidget(self.cb_replace)

        # --- 4. Чекбокс: Пробелы между символами ---
        self.cb_spaces = QCheckBox("Вставить по пробелу между символами")
        main_layout.addWidget(self.cb_spaces)

        # --- 5. Чекбокс: Сортировка слов ---
        self.cb_sort = QCheckBox("Сортировать слова в строке")
        main_layout.addWidget(self.cb_sort)

        # Радио-кнопки для выбора типа сортировки
        radio_layout = QVBoxLayout()
        radio_layout.setContentsMargins(30, 0, 0, 0)  # Отступ слева (как на рисунке)

        self.rb_size = QRadioButton("По размеру")
        self.rb_lex = QRadioButton("Лексикографически")
        self.rb_lex.setChecked(True)  # По умолчанию выбрано лексикографически

        # Группируем радио-кнопки, чтобы можно было выбрать только одну
        self.btn_group = QButtonGroup()
        self.btn_group.addButton(self.rb_size)  # ИСПРАВЛЕНО ЗДЕСЬ
        self.btn_group.addButton(self.rb_lex)  # ИСПРАВЛЕНО ЗДЕСЬ

        radio_layout.addWidget(self.rb_size)
        radio_layout.addWidget(self.rb_lex)
        main_layout.addLayout(radio_layout)

        # --- 6. Кнопка "Форматировать" ---
        self.btn_format = QPushButton("Форматировать")
        self.btn_format.clicked.connect(self.format_text)
        main_layout.addWidget(self.btn_format)

        # --- 7. Поле вывода результата ---
        output_layout = QHBoxLayout()
        self.lbl_output = QLabel("Результат:")
        self.le_output = QLineEdit()
        self.le_output.setReadOnly(True)  # Поле только для чтения
        output_layout.addWidget(self.lbl_output)
        output_layout.addWidget(self.le_output)
        main_layout.addLayout(output_layout)

        self.setLayout(main_layout)

        # Привязываем доступность радио-кнопок к галочке "Сортировать"
        self.cb_sort.toggled.connect(self.toggle_radios)
        self.toggle_radios(self.cb_sort.isChecked())  # Устанавливаем начальное состояние

    def toggle_radios(self, checked):
        """Включает/выключает радио-кнопки в зависимости от галочки сортировки."""
        self.rb_size.setEnabled(checked)
        self.rb_lex.setEnabled(checked)

    def format_text(self):
        """Сборка цепочки форматирования при нажатии на кнопку."""
        text = self.le_input.text()

        # 1. Удаление коротких слов
        if self.cb_remove.isChecked():
            text = StringFormatter.remove_short_words(text, self.spin_n.value())

        # 2. Замена цифр
        if self.cb_replace.isChecked():
            text = StringFormatter.replace_digits(text)

        # 3. Сортировка (должна идти ДО пробелов)
        if self.cb_sort.isChecked():
            if self.rb_size.isChecked():
                text = StringFormatter.sort_by_size(text)
            elif self.rb_lex.isChecked():
                text = StringFormatter.sort_lexicographically(text)

        # 4. Вставка пробелов (разбивает слова на буквы, поэтому выполняется последней)
        if self.cb_spaces.isChecked():
            text = StringFormatter.insert_spaces(text)

        # Выводим результат
        self.le_output.setText(text)


if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = FormatterApp()
    window.show()
    sys.exit(app.exec())