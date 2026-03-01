def make_capslock():
    my_text = input("Введите текст: ")
    res = []
    i = 0
    n = len(my_text)

    while i < n:
        ch = my_text[i]
        if ch.isalpha():
            token_chars = []
            while i < n:
                c = my_text[i]
                if c.isalpha():
                    token_chars.append(c)
                    i += 1
                elif c == '-' and token_chars and i + 1 < n and my_text[i + 1].isalpha():
                    token_chars.append(c)
                    i += 1
                else:
                    break
            word = ''.join(token_chars)
            if word and word[0].isupper():
                res.append(word.upper())
            else:
                res.append(word)
        else:
            res.append(ch)
            i += 1

    print(''.join(res))

make_capslock()