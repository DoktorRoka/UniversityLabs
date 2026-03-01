from functools import wraps
from typing import Iterable, Any

def pre_process(a: float = 0.97):
    """
    Параметризованный декоратор: применяет предобработку s[i] = s[i] - a * s[i-1]
    к входному списку/итерируемому объекту и передаёт обработанный список в
    декорируемую функцию.
    По умолчанию a = 0.97.
    """
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            if 's' in kwargs:
                seq_in = list(kwargs['s'])
                target_is_kw = True
            elif len(args) >= 1:
                seq_in = list(args[0])
                target_is_kw = False
            else:
                return func(*args, **kwargs)

            if not seq_in:
                processed = []
            else:
                processed = [float(seq_in[0])]
                for i in range(1, len(seq_in)):
                    processed.append(float(seq_in[i]) - a * float(seq_in[i-1]))

            if target_is_kw:
                kwargs = dict(kwargs)
                kwargs['s'] = processed
                return func(*args, **kwargs)
            else:
                new_args = (processed,) + args[1:]
                return func(*new_args, **kwargs)

        return wrapper
    return decorator

@pre_process(a=0.93)
def plot_signal(s):
    for sample in s:
        print(sample)

if __name__ == "__main__":
    x = [1.0, 3.0, 4.0, 2.0]
    plot_signal(x)
