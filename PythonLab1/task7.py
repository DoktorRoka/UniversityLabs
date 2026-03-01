def check_link():
    my_urls = input("Введите адреса через запятую: ").split(",")

    result = [
        (
            ("http://" + u.strip() if u.strip().startswith("www") else u.strip())
            if (("http://" + u.strip() if u.strip().startswith("www") else u.strip()).endswith(".com"))
            else
            (("http://" + u.strip() if u.strip().startswith("www") else u.strip()) + ".com")
        )
        for u in my_urls
    ]

    print(result)


check_link()