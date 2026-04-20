import pandas as pd
import os
import ast
import sys
sys.stdout.reconfigure(encoding="utf-8")

CSV_URL = "https://raw.githubusercontent.com/mledoze/countries/master/dist/countries.csv"
CSV_LOCAL = "countries.csv"


def load_data():
    if not os.path.exists(CSV_LOCAL):
        print("Downloading countries.csv...")
        df = pd.read_csv(CSV_URL)
        df.to_csv(CSV_LOCAL, index=False)
        print("Saved to countries.csv")
    else:
        df = pd.read_csv(CSV_LOCAL)
    return df


def parse_latlng(val):
    try:
        parts = str(val).split(",")
        lat = float(parts[0].strip())
        lon = float(parts[1].strip()) if len(parts) > 1 else None
        return lat, lon
    except (ValueError, IndexError, TypeError):
        return None, None


def parse_currencies(val):
    try:
        d = ast.literal_eval(str(val))
        if isinstance(d, dict):
            return ", ".join(d.keys())
        return str(val)
    except Exception:
        return str(val)


def task_1_top_bottom_area(df):
    print("\n=== 1) Top 10 largest and smallest countries by area ===")
    valid = df[df["area"] > 0].sort_values("area", ascending=False)
    print("\n10 Largest:")
    print(valid[["name.common", "area"]].head(10).to_string(index=False))
    print("\n10 Smallest:")
    print(valid[["name.common", "area"]].tail(10).to_string(index=False))


def task_2_top_bottom_population(df):
    print("\n=== 2) Top 10 largest and smallest countries by population ===")
    df_pop = df.copy()
    if "population" not in df_pop.columns:
        print("Population column not found in CSV. Skipping.")
        return
    valid = df_pop[df_pop["population"] > 0].sort_values("population", ascending=False)
    print("\n10 Largest by population:")
    print(valid[["name.common", "population"]].head(10).to_string(index=False))
    print("\n10 Smallest by population:")
    print(valid[["name.common", "population"]].tail(10).to_string(index=False))


def task_3_francophone(df):
    print("\n=== 3) Francophone countries ===")
    mask = df["languages"].str.contains("French", case=False, na=False)
    fr_countries = df[mask]["name.common"].tolist()
    print(f"Found {len(fr_countries)} francophone countries:")
    for c in sorted(fr_countries):
        print(f"  {c}")


def task_4_island_countries(df):
    print("\n=== 4) Island countries (landlocked=False, no land borders) ===")
    mask = (df["landlocked"] == False) & (df["borders"].isna() | (df["borders"] == ""))
    islands = df[mask]["name.common"].tolist()
    print(f"Found {len(islands)} island countries:")
    for c in sorted(islands):
        print(f"  {c}")


def task_5_southern_hemisphere(df):
    print("\n=== 5) Countries in the Southern Hemisphere ===")
    df_south = df.copy()
    lats = df_south["latlng"].apply(parse_latlng)
    df_south["latitude"] = lats.apply(lambda x: x[0] if x else None)
    south = df_south[df_south["latitude"] < 0]["name.common"].tolist()
    print(f"Found {len(south)} countries in the Southern Hemisphere:")
    for c in sorted(south):
        print(f"  {c}")


def task_6_groupings(df):
    print("\n=== 6) Groupings ===")

    print("\n--- Group by first letter ---")
    df_copy = df.copy()
    df_copy["first_letter"] = df_copy["name.common"].str[0].str.upper()
    grouped = df_copy.groupby("first_letter")["name.common"].count()
    for letter, count in grouped.items():
        print(f"  {letter}: {count} countries")

    print("\n--- Group by population ranges ---")
    if "population" in df_copy.columns:
        bins = [0, 1000000, 10000000, 100000000, float("inf")]
        labels_pop = ["<1M", "1M-10M", "10M-100M", ">100M"]
        df_copy["pop_group"] = pd.cut(df_copy["population"], bins=bins, labels=labels_pop, right=False)
        print(df_copy.groupby("pop_group", observed=False)["name.common"].count())
    else:
        print("  (no population data)")

    print("\n--- Group by area ranges ---")
    bins_a = [0, 1000, 100000, 1000000, float("inf")]
    labels_a = ["<1K km2", "1K-100K km2", "100K-1M km2", ">1M km2"]
    df_copy["area_group"] = pd.cut(df_copy["area"], bins=bins_a, labels=labels_a, right=False)
    print(df_copy.groupby("area_group", observed=False)["name.common"].count())


def task_7_excel_export(df):
    print("\n=== 7) Export to Excel ===")
    df_exp = df.copy()
    lats = df_exp["latlng"].apply(parse_latlng)
    df_exp["latitude"] = lats.apply(lambda x: x[0] if x else None)
    df_exp["longitude"] = lats.apply(lambda x: x[1] if x and len(x) > 1 else None)
    df_exp["currency"] = df_exp["currencies"].apply(parse_currencies)

    export_cols = ["name.common", "capital", "population", "area", "currency", "latitude", "longitude"]
    available_cols = [c for c in export_cols if c in df_exp.columns]
    result = df_exp[available_cols].copy()
    result.columns = ["Name", "Capital", "Population", "Area", "Currency", "Latitude", "Longitude"][:len(available_cols)]

    output_file = "countries_export.xlsx"
    try:
        result.to_excel(output_file, index=False)
        print(f"Exported {len(result)} countries to {output_file}")
    except ImportError:
        result.to_csv("countries_export.csv", index=False)
        print(f"openpyxl not installed. Exported as countries_export.csv instead")


if __name__ == "__main__":
    df = load_data()
    print(f"Loaded {len(df)} countries")
    print(f"Columns: {list(df.columns)}")

    task_1_top_bottom_area(df)
    task_2_top_bottom_population(df)
    task_3_francophone(df)
    task_4_island_countries(df)
    task_5_southern_hemisphere(df)
    task_6_groupings(df)
    task_7_excel_export(df)
