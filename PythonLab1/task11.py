def frange(start, stop, step):
    x = start
    while x < stop:
        yield round(x, 10)
        x += step


for x in frange(1, 5, 0.1):
    print(x)