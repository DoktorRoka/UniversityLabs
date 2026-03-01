def get_frames(signal, size, overlap):
    step = int(size * (1 - overlap))

    if step <= 0:
        raise ValueError("Некорректное значение overlap")

    i = 0
    while i + size <= len(signal):
        yield signal[i:i + size]
        i += step


signal = list(range(20))

for frame in get_frames(signal, size=6, overlap=0.5):
    print(frame)