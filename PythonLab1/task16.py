import random
from datetime import date, timedelta, datetime

def round_robin(teams):

    teams = list(teams)
    if len(teams) % 2 != 0:
        teams.append(None)
    n = len(teams)
    rounds = []
    for r in range(n - 1):
        pairs = []
        for i in range(n // 2):
            t1 = teams[i]
            t2 = teams[n - 1 - i]
            if t1 is not None and t2 is not None:
                pairs.append((t1, t2))
        rounds.append(pairs)
        teams = [teams[0]] + [teams[-1]] + teams[1:-1]
    return rounds

def first_wednesday_on_or_after(year, month, day):
    d = date(year, month, day)
    delta = (2 - d.weekday()) % 7
    return d + timedelta(days=delta)

def generate_groups_and_calendar(teams=None, seed=None):

    if seed is not None:
        random.seed(seed)
    default_teams = [
        "Спартак", "Зенит", "ЦСКА", "Локомотив", "Динамо", "Ростов", "Рубин", "Краснодар",
        "Арсенал", "Крылья Советов", "Урал", "Сочи", "Нижний Новгород", "Ахмат", "Оренбург", "Томь"
    ]
    if teams is None:
        teams = default_teams
    teams = list(teams)
    if len(teams) != 16:
        raise ValueError("Ожидается список из 16 команд.")

    random.shuffle(teams)
    groups = [teams[i*4:(i+1)*4] for i in range(4)]

    print("Группы (случайно сформированы):")
    for gi, g in enumerate(groups, 1):
        print(f"Группа {gi}: {', '.join(g)}")
    print()

    group_rounds = [round_robin(g) for g in groups]  # each is list of rounds

    year = datetime.now().year
    first_md = first_wednesday_on_or_after(year, 9, 14)
    matchdays = [first_md + timedelta(weeks=2*i) for i in range(len(group_rounds[0]))]

    print("Календарь матчей (дата, время 22:45):")
    for rd_idx, md in enumerate(matchdays):
        md_str = md.strftime("%d/%m/%Y, 22:45")
        print(f"\nМатчдень {rd_idx+1}: {md_str}")
        for gi, rounds in enumerate(group_rounds, 1):
            matches = rounds[rd_idx]
            for a, b in matches:
                print(f"  Группа {gi}: {a} — {b}")
    print()

if __name__ == "__main__":
    generate_groups_and_calendar(seed=42)