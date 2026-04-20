import tkinter as tk
from tkinter import ttk, messagebox
import threading
import requests
import os
import time
import matplotlib.pyplot as plt


class FileDownloaderApp:
    def __init__(self):
        self.root = tk.Tk()
        self.root.title("File Downloader")
        self.root.geometry("700x400")
        self.root.resizable(False, False)

        self.url_entries = []
        self.progress_bars = []
        self.percent_labels = []
        self.status_labels = []
        self.start_times = [None, None, None]
        self.end_times = [None, None, None]
        self.file_sizes = [0, 0, 0]
        self.download_flags = [False, False, False]
        self.completed = [False, False, False]

        for i in range(3):
            frame = tk.LabelFrame(self.root, text=f"File {i + 1}", padx=10, pady=5)
            frame.pack(fill="x", padx=10, pady=5)

            url_entry = tk.Entry(frame, width=80)
            url_entry.pack(fill="x")
            self.url_entries.append(url_entry)

            prog_frame = tk.Frame(frame)
            prog_frame.pack(fill="x", pady=2)

            progress = ttk.Progressbar(prog_frame, length=500, mode="determinate")
            progress.pack(side="left", fill="x", expand=True)
            self.progress_bars.append(progress)

            percent_label = tk.Label(prog_frame, text="0%", width=6)
            percent_label.pack(side="left", padx=5)
            self.percent_labels.append(percent_label)

            status_label = tk.Label(frame, text="Idle", anchor="w")
            status_label.pack(fill="x")
            self.status_labels.append(status_label)

        self.start_btn = tk.Button(self.root, text="Start downloading!",
                                   command=self.start_downloading, bg="green", fg="white",
                                   font=("Arial", 12, "bold"))
        self.start_btn.pack(pady=10)

    def start_downloading(self):
        self.start_btn.config(state="disabled")
        self.completed = [False, False, False]
        self.start_times = [None, None, None]
        self.end_times = [None, None, None]
        self.file_sizes = [0, 0, 0]

        active_count = 0
        for i in range(3):
            url = self.url_entries[i].get().strip()
            if url:
                active_count += 1
                self.progress_bars[i]["value"] = 0
                self.percent_labels[i].config(text="0%")
                self.status_labels[i].config(text="Starting...")
                self.download_flags[i] = True
                t = threading.Thread(target=self.download_file, args=(i, url), daemon=True)
                t.start()
            else:
                self.progress_bars[i]["value"] = 0
                self.percent_labels[i].config(text="0%")
                self.status_labels[i].config(text="Idle")
                self.completed[i] = True

    def download_file(self, index, url):
        download_dir = os.path.join(os.path.dirname(__file__), "downloads")
        os.makedirs(download_dir, exist_ok=True)
        filename = url.split("/")[-1] or f"file_{index + 1}"
        filepath = os.path.join(download_dir, filename)

        try:
            self.start_times[index] = time.time()
            response = requests.get(url, stream=True, timeout=30)
            response.raise_for_status()
            total_size = int(response.headers.get("content-length", 0))
            self.file_sizes[index] = total_size
            downloaded = 0

            with open(filepath, "wb") as f:
                for chunk in response.iter_content(chunk_size=4096):
                    if not self.download_flags[index]:
                        break
                    if chunk:
                        f.write(chunk)
                        downloaded += len(chunk)
                        if total_size > 0:
                            percent = int(downloaded / total_size * 100)
                            self.root.after(0, self._update_progress, index, percent, downloaded, total_size)
                        else:
                            self.root.after(0, self._update_status, index, f"Downloaded {downloaded} bytes")

            self.end_times[index] = time.time()
            self.completed[index] = True
            self.root.after(0, self._update_progress, index, 100, downloaded, total_size if total_size > 0 else downloaded)
            self.root.after(0, self._update_status, index, "Done!")
            self.root.after(0, self._check_all_done)

        except Exception as e:
            self.end_times[index] = time.time()
            self.completed[index] = True
            self.root.after(0, self._update_status, index, f"Error: {e}")
            self.root.after(0, self._check_all_done)

    def _update_progress(self, index, percent, downloaded, total):
        self.progress_bars[index]["value"] = percent
        self.percent_labels[index].config(text=f"{percent}%")
        if total > 0:
            self.status_labels[index].config(text=f"{downloaded}/{total} bytes")

    def _update_status(self, index, text):
        self.status_labels[index].config(text=text)

    def _check_all_done(self):
        if all(self.completed):
            self.start_btn.config(state="normal")
            self._show_results_chart()

    def _show_results_chart(self):
        labels = []
        times_s = []
        sizes_kb = []

        for i in range(3):
            url = self.url_entries[i].get().strip()
            if url and self.start_times[i] is not None and self.end_times[i] is not None:
                elapsed = self.end_times[i] - self.start_times[i]
                elapsed_ms = elapsed * 1000
                s = int(elapsed_ms // 1000)
                ms = int(elapsed_ms % 1000)
                labels.append(f"File {i + 1}\n{s}s {ms}ms")
                times_s.append(elapsed)
                sizes_kb.append(self.file_sizes[i] / 1024)

        if not labels:
            return

        fig, (ax1, ax2) = plt.subplots(1, 2, figsize=(10, 5))

        x = range(len(labels))
        bars1 = ax1.bar(x, times_s, color="steelblue", tick_label=labels)
        ax1.set_ylabel("Time (seconds)")
        ax1.set_title("Download Time")
        for bar, t in zip(bars1, times_s):
            elapsed_ms = t * 1000
            s = int(elapsed_ms // 1000)
            ms = int(elapsed_ms % 1000)
            ax1.text(bar.get_x() + bar.get_width() / 2, bar.get_height(),
                     f"{s}s {ms}ms", ha="center", va="bottom", fontsize=9)

        bars2 = ax2.bar(x, sizes_kb, color="coral", tick_label=labels)
        ax2.set_ylabel("Size (KB)")
        ax2.set_title("File Size")
        for bar, sz in zip(bars2, sizes_kb):
            ax2.text(bar.get_x() + bar.get_width() / 2, bar.get_height(),
                     f"{sz:.1f} KB", ha="center", va="bottom", fontsize=9)

        plt.tight_layout()
        plt.show()

    def run(self):
        self.root.mainloop()


if __name__ == "__main__":
    app = FileDownloaderApp()
    app.run()
