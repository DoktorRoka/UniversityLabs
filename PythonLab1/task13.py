from typing import Iterable, Iterator, Tuple

def extra_enumerate(seq: Iterable[float], start: int = 0) -> Iterator[Tuple[int, float, float, float]]:
    items = list(seq)
    total = sum(items)
    cum = 0
    for idx, elem in enumerate(items, start):
        cum += elem
        frac = (cum / total) if total != 0 else 0.0
        yield idx, elem, cum, frac


x = [1, 3, 4, 2]

for i, elem, cum, frac in extra_enumerate(x):
    print((elem, cum, round(frac, 3)))