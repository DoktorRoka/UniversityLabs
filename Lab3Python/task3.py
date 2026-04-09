import sys
import os
import re
from datetime import datetime

# Импорты для PyQt6
from PyQt6.QtWidgets import (QApplication, QMainWindow, QListWidget,
                             QFileDialog, QMessageBox, QLabel)
from PyQt6.QtGui import QAction


class SearchApp(QMainWindow):
    def __init__(self):
        super().__init__()

        # Настройки главного окна
        self.setWindowTitle("Скрипт Искатель строк")
        self.resize(600, 400)

        self.log_filename = "script.log"

        # Настройка интерфейса
        self.init_ui()
        # Проверка наличия файла лога
        self.check_log_file()

    def init_ui(self):
        # 1. Центральный виджет - список на всю область окна
        self.list_widget = QListWidget()
        self.setCentralWidget(self.list_widget)

        # 2. Создание меню
        menubar = self.menuBar()

        # Меню "Файл"
        file_menu = menubar.addMenu("Файл")
        open_action = QAction("Открыть...", self)
        open_action.triggered.connect(self.open_file)
        file_menu.addAction(open_action)

        # Меню "Лог"
        log_menu = menubar.addMenu("Лог")

        export_action = QAction("Экспорт...", self)
        export_action.triggered.connect(self.export_log)

        append_action = QAction("Добавить в лог", self)
        append_action.triggered.connect(self.append_to_log)

        view_action = QAction("Просмотр", self)
        view_action.triggered.connect(self.view_log)

        log_menu.addAction(export_action)
        log_menu.addAction(append_action)
        log_menu.addAction(view_action)

        # 3. Настройка статусной строки
        statusbar = self.statusBar()

        # Создаем два поля для статусной строки
        self.status_action = QLabel("")
        self.status_size = QLabel("")

        # Добавляем их с пропорциями растяжения (6 к 4, то есть 60% и 40%)
        statusbar.addWidget(self.status_action, 6)
        statusbar.addWidget(self.status_size, 4)

    def check_log_file(self):
        """Проверяет существование лог-файла при запуске."""
        if not os.path.exists(self.log_filename):
            QMessageBox.information(
                self,
                "Информация",
                "Файл лога не найден. Файл будет создан автоматически"
            )
            # Создаем пустой файл
            with open(self.log_filename, 'w', encoding='utf-8') as f:
                pass

    def open_file(self):
        """Открывает диалог выбора файла и запускает поиск."""
        filepath, _ = QFileDialog.getOpenFileName(self, "Выберите файл для поиска", "",
                                                  "Text Files (*.txt);;All Files (*)")

        if filepath:
            self.process_file(filepath)

    def process_file(self, filepath):
        """Функция поиска строк по шаблону (ИНТЕГРИРОВАН ТВОЙ КОД)."""

        # Компилируем твое регулярное выражение для поиска номеров
        pattern = re.compile(r'\(\d{3}\)(?:\d{7}|\d{3}-\d{2}-\d{2})')

        now_str = datetime.now().strftime("%d.%m.%Y %H:%M:%S")
        self.list_widget.addItem(f"Файл {filepath} был обработан {now_str}:")

        matches_found = False
        try:
            with open(filepath, 'r', encoding='utf-8') as file:
                # Твой цикл прохода по файлу
                for line_num, line in enumerate(file, start=1):
                    for match in pattern.finditer(line):
                        matches_found = True
                        pos = match.start()
                        found_text = match.group()

                        # Вывод в ListWidget вместо print
                        self.list_widget.addItem(
                            f"Строка {line_num}, позиция {pos} : найдено «{found_text}»"
                        )

            if not matches_found:
                self.list_widget.addItem("В файле не найдено ни одного подходящего номера телефона.")

        except Exception as e:
            self.list_widget.addItem(f"Ошибка чтения файла: {e}")

        self.list_widget.addItem("")  # Пустая строка для визуального отступа между поисками

        # Обновление статусной строки (форматирование пробелами тысяч)
        size_bytes = os.path.getsize(filepath)
        size_str = f"{size_bytes:,}".replace(',', ' ') + " байт"

        self.status_action.setText(f"Обработан файл {filepath}")
        self.status_size.setText(size_str)

    def export_log(self):
        """Сохраняет текущее содержимое списка в выбранный файл."""
        filepath, _ = QFileDialog.getSaveFileName(self, "Экспорт лога", "", "Text Files (*.txt);;All Files (*)")
        if filepath:
            with open(filepath, 'w', encoding='utf-8') as f:
                for i in range(self.list_widget.count()):
                    f.write(self.list_widget.item(i).text() + '\n')

    def append_to_log(self):
        """Добавляет текущее содержимое списка в конец script18.log."""
        with open(self.log_filename, 'a', encoding='utf-8') as f:
            for i in range(self.list_widget.count()):
                f.write(self.list_widget.item(i).text() + '\n')
        self.status_action.setText("Текущие результаты добавлены в лог")

    def view_log(self):
        """Очищает список и выводит данные из лога с подтверждением."""
        # Диалоговое окно подтверждения (в PyQt6 используем QMessageBox.StandardButton)
        reply = QMessageBox.question(
            self, 'Внимание',
            "Вы действительно хотите открыть лог? Данные последних поисков будут потеряны!",
            QMessageBox.StandardButton.Yes | QMessageBox.StandardButton.No,
            QMessageBox.StandardButton.No
        )

        if reply == QMessageBox.StandardButton.Yes:
            self.list_widget.clear()
            if os.path.exists(self.log_filename):
                with open(self.log_filename, 'r', encoding='utf-8') as f:
                    for line in f:
                        self.list_widget.addItem(line.strip('\n'))

            self.status_action.setText("Открыт лог")
            self.status_size.setText("")  # При просмотре лога размер файла сбрасывается


if __name__ == '__main__':
    app = QApplication(sys.argv)
    window = SearchApp()
    window.show()
    sys.exit(app.exec())  # В PyQt6 exec() вместо exec_()