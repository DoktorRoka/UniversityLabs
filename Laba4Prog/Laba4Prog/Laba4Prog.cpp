#ifdef _WIN32
#ifndef NOMINMAX
#define NOMINMAX
#endif
#include <windows.h>
#endif

#include <algorithm>
#include <array>
#include <cctype>
#include <iomanip>
#include <iostream>
#include <limits>
#include <random>
#include <sstream>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

using std::cout;
using std::cin;
using std::string;

struct IFormattable {
    virtual ~IFormattable() = default;
    virtual std::string format() const = 0;
};

void prettyPrint(const IFormattable& object) {
    std::cout << object.format() << "\n";
}

// -------------------- Карты --------------------
enum class Suit { Spades, Hearts, Diamonds, Clubs };

static inline const char* suitUnicode(Suit s) {
    // Unicode: ♥ ♦ ♣ ♠
    switch (s) {
    case Suit::Hearts:   return u8"\u2665";
    case Suit::Diamonds: return u8"\u2666";
    case Suit::Clubs:    return u8"\u2663";
    case Suit::Spades:   return u8"\u2660";
    }
    return "?";
}

struct Card {
    std::string rank;
    Suit suit{ Suit::Spades };

    Card() = default;
    Card(std::string r, Suit s) : rank(std::move(r)), suit(s) {}

    bool isAce() const { return rank == "A"; }

    int baseValue() const {
        if (rank == "A") return 11;
        if (rank == "K" || rank == "Q" || rank == "J") return 10;
        return std::stoi(rank); // 2..10 or 6..10
    }

    friend std::ostream& operator<<(std::ostream& os, const Card& c) {
        os << c.rank << suitUnicode(c.suit);
        return os;
    }
};

class Deck {
public:
    explicit Deck(int deckSize = 52) { reset(deckSize); }

    void reset(int deckSize) {
        if (deckSize != 52 && deckSize != 36) {
            throw std::invalid_argument("Deck size must be 36 or 52");
        }
        fullSize_ = deckSize;
        cards_.clear();
        cards_.reserve(static_cast<size_t>(deckSize));

        std::vector<std::string> ranks;
        if (deckSize == 52) {
            ranks = { "2","3","4","5","6","7","8","9","10","J","Q","K","A" };
        }
        else { // 36 cards (common "Russian" set: 6..A)
            ranks = { "6","7","8","9","10","J","Q","K","A" };
        }

        for (Suit s : {Suit::Spades, Suit::Hearts, Suit::Diamonds, Suit::Clubs}) {
            for (const auto& r : ranks) {
                cards_.emplace_back(r, s);
            }
        }
        shuffle();
    }

    void shuffle() {
        static thread_local std::mt19937 rng{ std::random_device{}() };
        std::shuffle(cards_.begin(), cards_.end(), rng);
    }

    bool empty() const { return cards_.empty(); }
    size_t size() const { return cards_.size(); }
    int fullSize() const { return fullSize_; }

    Card draw() {
        if (cards_.empty()) throw std::runtime_error("Deck is empty");
        Card c = cards_.back();
        cards_.pop_back();
        return c;
    }

    std::vector<Card> topCards(size_t n) const {
        std::vector<Card> out;
        n = std::min(n, cards_.size());
        out.reserve(n);
        for (size_t i = 0; i < n; ++i) {
            out.push_back(cards_[cards_.size() - 1 - i]); // top = back
        }
        return out;
    }

private:
    int fullSize_{ 52 };
    std::vector<Card> cards_;
};

class Shoe {
public:
    Shoe(int deckCount, int deckSize)
        : rng_(std::random_device{}()) {
        if (deckCount <= 0) throw std::invalid_argument("deckCount must be > 0");
        decks_.reserve(static_cast<size_t>(deckCount));
        for (int i = 0; i < deckCount; ++i) {
            decks_.emplace_back(deckSize);
        }
    }

    Card draw() {
        std::vector<size_t> nonEmpty;
        for (size_t i = 0; i < decks_.size(); ++i) {
            if (!decks_[i].empty()) nonEmpty.push_back(i);
        }
        if (nonEmpty.empty()) {
            int deckSize = decks_.empty() ? 52 : decks_[0].fullSize();
            *this = Shoe(static_cast<int>(decks_.size()), deckSize);
            for (size_t i = 0; i < decks_.size(); ++i) {
                nonEmpty.push_back(i);
            }
        }
        std::uniform_int_distribution<size_t> dist(0, nonEmpty.size() - 1);
        size_t idx = nonEmpty[dist(rng_)];
        return decks_[idx].draw();
    }

    std::string countsLine() const {
        std::ostringstream ss;
        ss << "Колоды:";
        for (const auto& d : decks_) ss << " [" << d.size() << "]";
        return ss.str();
    }

    const Deck& deck(size_t i) const { return decks_.at(i); }
    Deck& deck(size_t i) { return decks_.at(i); }
    size_t deckCount() const { return decks_.size(); }

private:
    std::mt19937 rng_;
    std::vector<Deck> decks_;
};

class BustException : public std::runtime_error {
public:
    explicit BustException(int score)
        : std::runtime_error("Bust"), score_(score) {}
    int score() const { return score_; }
private:
    int score_{ 0 };
};

class Hand {
public:
    void clear() { cards_.clear(); }

    void addCard(const Card& c) {
        cards_.push_back(c);
        if (score() > 21) {
            throw BustException(score());
        }
    }

    size_t size() const { return cards_.size(); }

    const std::vector<Card>& cards() const { return cards_; }

    int score() const {
        int total = 0;
        int aces = 0;
        for (const auto& c : cards_) {
            total += c.baseValue();
            if (c.isAce()) ++aces;
        }
        while (total > 21 && aces > 0) {
            total -= 10; 
            --aces;
        }
        return total;
    }

    bool isBlackjack21() const {
        return cards_.size() == 2 && score() == 21;
    }

    bool isSpecial17Plus4() const {
        return cards_.size() == 4 && score() == 17;
    }

    std::string toString(bool hideSecondCard = false) const {
        std::ostringstream ss;
        for (size_t i = 0; i < cards_.size(); ++i) {
            if (hideSecondCard && i == 1) ss << "??";
            else ss << cards_[i];
            if (i + 1 != cards_.size()) ss << " ";
        }
        return ss.str();
    }

    Card takeCard(size_t idx) {
        Card c = cards_.at(idx);
        cards_.erase(cards_.begin() + static_cast<std::ptrdiff_t>(idx));
        return c;
    }

private:
    std::vector<Card> cards_;
};

// -------------------- участники --------------------
class Player {
public:
    explicit Player(long long money = 10000) : money_(money) {}

    long long money() const { return money_; }
    void addMoney(long long delta) { money_ += delta; }

    void resetHands() {
        hands_.clear();
        hands_.emplace_back();
        busted_.clear();
        busted_.push_back(false);
    }

    std::vector<Hand>& hands() { return hands_; }
    const std::vector<Hand>& hands() const { return hands_; }

    bool isBusted(size_t i) const { return busted_.at(i); }
    void setBusted(size_t i, bool v) { busted_.at(i) = v; }

    bool hasSplit() const { return hands_.size() > 1; }

    bool canSplitFirstHand() const {
        if (hands_.size() != 1) return false;
        const auto& h = hands_[0];
        if (h.size() != 2) return false;
        return h.cards()[0].rank == h.cards()[1].rank;
    }

    void doSplit() {
        if (!canSplitFirstHand()) return;
        Hand second;
        Card c2 = hands_[0].takeCard(1);
        second.addCard(c2); 
        hands_.push_back(second);
        busted_.push_back(false);
    }

private:
    long long money_{ 10000 };
    std::vector<Hand> hands_{ Hand{} };
    std::vector<bool> busted_{ false };
};

class Dealer {
public:
    void reset() { hand_.clear(); busted_ = false; }
    Hand& hand() { return hand_; }
    const Hand& hand() const { return hand_; }
    bool busted() const { return busted_; }
    void setBusted(bool v) { busted_ = v; }
private:
    Hand hand_;
    bool busted_{ false };
};


class DeckObjectAdapter : public IFormattable {
public:
    explicit DeckObjectAdapter(const Deck& deck) : deck_(deck) {}

    std::string format() const override {
        std::ostringstream ss;
        ss << "[Deck (object-adapter)] remaining=" << deck_.size()
            << ", top:";
        for (const auto& c : deck_.topCards(5)) ss << " " << c;
        return ss.str();
    }

private:
    const Deck& deck_;
};

class DeckClassAdapter : public Deck, public IFormattable {
public:
    explicit DeckClassAdapter(int deckSize = 52) : Deck(deckSize) {}

    std::string format() const override {
        std::ostringstream ss;
        ss << "[Deck (class-adapter)] remaining=" << size()
            << ", top:";
        for (const auto& c : topCards(5)) ss << " " << c;
        return ss.str();
    }
};

// -------------------- игра --------------------
class Game {
public:
    Game()
        : rng_(std::random_device{}()),
        shoe_(4, 52),
        player_(10000) {
    }

    void run() {
        cout << "BlackJack (лаба №4). Вариант 5: \"17 + 4\", тип: базовый, доп. правило: сплит.\n";
        cout << "Демонстрация адаптера (задание 2):\n";
        DeckObjectAdapter oa(shoe_.deck(0));
        prettyPrint(oa);
        DeckClassAdapter ca(52);
        prettyPrint(ca);
        cout << "\n";

        while (player_.money() > 0) {
            long long bet = askBet();
            playRound(bet);

            cout << "\nСыграть еще? (1 - да, 0 - нет): ";
            int again = 0;
            cin >> again;
            if (!cin || again == 0) break;
        }

        cout << "\nИгра окончена. Ваш баланс: " << player_.money() << "\n";
    }

private:
    long long askBet() {
        while (true) {
            cout << "Ваша ставка?\n";
            long long bet = 0;
            cin >> bet;
            if (!cin) {
                cin.clear();
                cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                continue;
            }
            if (bet <= 0) continue;
            if (bet > player_.money()) {
                cout << "Недостаточно средств. Ваш баланс: " << player_.money() << "\n";
                continue;
            }
            return bet;
        }
    }

    void initialDealBasic() {
        dealer_.reset();
        player_.resetHands();

        // Dealer 2 cards, second hidden in output
        try {
            dealer_.hand().addCard(shoe_.draw());
            dealer_.hand().addCard(shoe_.draw());
        }
        catch (const BustException&) {
        }

        try {
            player_.hands()[0].addCard(shoe_.draw());
            player_.hands()[0].addCard(shoe_.draw());
        }
        catch (const BustException&) {
        }
    }

    void showTableState(bool revealDealer = false) const {
        cout << shoe_.countsLine() << "\n";
        cout << "Дилер: " << dealer_.hand().toString(!revealDealer) << "\n";

        if (player_.hands().size() == 1) {
            cout << "Вы: " << player_.hands()[0].toString(false) << "\n";
        }
        else {
            for (size_t i = 0; i < player_.hands().size(); ++i) {
                cout << "Вы (рука " << (i + 1) << "): " << player_.hands()[i].toString(false) << "\n";
            }
        }
    }

    bool isAnyPlayerSpecialOrBlackjack() const {
        for (size_t i = 0; i < player_.hands().size(); ++i) {
            const auto& h = player_.hands()[i];
            if (!player_.isBusted(i) && (h.isBlackjack21() || h.isSpecial17Plus4())) return true;
        }
        return false;
    }

    void playPlayerHands() {
        for (size_t handIdx = 0; handIdx < player_.hands().size(); ++handIdx) {
            while (true) {
                auto& hand = player_.hands()[handIdx];

                if (hand.isBlackjack21()) {
                    cout << "У вас БЛЕК-ДЖЕК (21 за 2 карты)!\n";
                    break;
                }
                if (hand.isSpecial17Plus4()) {
                    cout << "У вас спец. БЛЕК-ДЖЕК \"17 + 4\"!\n";
                    break;
                }

                showTableState(false);

                cout << "1. Хватит\n";
                cout << "2. Еще\n";
                if (handIdx == 0 && player_.canSplitFirstHand()) {
                    cout << "3. Сплит?\n";
                }

                int choice = 0;
                cin >> choice;
                if (!cin) {
                    cin.clear();
                    cin.ignore(std::numeric_limits<std::streamsize>::max(), '\n');
                    continue;
                }

                if (choice == 1) {
                    break;
                }
                else if (choice == 2) {
                    try {
                        hand.addCard(shoe_.draw());
                    }
                    catch (const BustException& ex) {
                        player_.setBusted(handIdx, true);
                        showTableState(false);
                        cout << "Перебор! (" << ex.score() << ")\n";
                        break;
                    }
                }
                else if (choice == 3 && handIdx == 0 && player_.canSplitFirstHand()) {
                    player_.doSplit();
                    for (size_t i = 0; i < player_.hands().size(); ++i) {
                        try {
                            player_.hands()[i].addCard(shoe_.draw());
                        }
                        catch (const BustException& ex) {
                            player_.setBusted(i, true);
                            cout << "Перебор на руке " << (i + 1) << "! (" << ex.score() << ")\n";
                        }
                    }
                }
            }
        }
    }

    void playDealerBasic() {
        showTableState(true);

        bool anyAlive = false;
        for (size_t i = 0; i < player_.hands().size(); ++i) {
            if (!player_.isBusted(i)) { anyAlive = true; break; }
        }
        if (!anyAlive) return;

        // Dealer draws until >= 17
        while (dealer_.hand().score() < 17) {
            cout << "Дилер берет карту...\n";
            try {
                dealer_.hand().addCard(shoe_.draw());
            }
            catch (const BustException& ex) {
                dealer_.setBusted(true);
                showTableState(true);
                cout << "У дилера перебор! (" << ex.score() << ")\n";
                return;
            }
            showTableState(true);
        }
    }

    enum class Outcome { Win, Lose, Push };

    Outcome compareHandToDealer(const Hand& ph) const {
        if (dealer_.busted()) return Outcome::Win;

        const int p = ph.score();
        const int d = dealer_.hand().score();

        // Blackjack handling:
        const bool pBJ = ph.isBlackjack21() || ph.isSpecial17Plus4();
        const bool dBJ = dealer_.hand().isBlackjack21() || dealer_.hand().isSpecial17Plus4();

        if (pBJ && dBJ) return Outcome::Push;
        if (pBJ && !dBJ) return Outcome::Win;
        if (!pBJ && dBJ) return Outcome::Lose;

        if (p > d) return Outcome::Win;
        if (p < d) return Outcome::Lose;
        return Outcome::Push;
    }

    void settleBet(long long bet) {
        const bool split = (player_.hands().size() == 2);

        if (!split) {
            if (player_.isBusted(0)) {
                player_.addMoney(-bet);
                cout << "Вы проиграли! Проигрыш: " << bet << ". Всего: " << player_.money() << ".\n";
                return;
            }
            Outcome o = compareHandToDealer(player_.hands()[0]);
            if (o == Outcome::Win) {
                player_.addMoney(+bet);
                cout << "Поздравляем! Вы выиграли! Выигрыш: " << bet << ". Всего: " << player_.money() << ".\n";
            }
            else if (o == Outcome::Lose) {
                player_.addMoney(-bet);
                cout << "Вы проиграли! Проигрыш: " << bet << ". Всего: " << player_.money() << ".\n";
            }
            else {
                cout << "Ровно (push). Ставка возвращается. Всего: " << player_.money() << ".\n";
            }
            return;
        }

        // Split payout per lab text:
        // - if both hands win => win is doubled ( +2*bet )
        // - if one wins and one loses => player stays with bet ( 0 )
        // - if both lose => stake is lost ( -bet ), NOT -2*bet
        int wins = 0, losses = 0, pushes = 0;
        for (size_t i = 0; i < 2; ++i) {
            if (player_.isBusted(i)) { ++losses; continue; }
            Outcome o = compareHandToDealer(player_.hands()[i]);
            if (o == Outcome::Win) ++wins;
            else if (o == Outcome::Lose) ++losses;
            else ++pushes;
        }

        long long delta = 0;
        if (wins == 2) delta = +2 * bet;
        else if (losses == 2 && wins == 0 && pushes == 0) delta = -bet; // special lab rule
        else delta = static_cast<long long>(wins - losses) * bet;

        player_.addMoney(delta);

        // Print per-hand result
        for (size_t i = 0; i < 2; ++i) {
            cout << "Рука " << (i + 1) << ": ";
            if (player_.isBusted(i)) {
                cout << "перебор -> проигрыш\n";
                continue;
            }
            Outcome o = compareHandToDealer(player_.hands()[i]);
            if (o == Outcome::Win) cout << "выигрыш\n";
            else if (o == Outcome::Lose) cout << "проигрыш\n";
            else cout << "ровно\n";
        }

        if (delta > 0) {
            cout << "Итог: вы выиграли " << delta << ". Всего: " << player_.money() << ".\n";
        }
        else if (delta < 0) {
            cout << "Итог: вы проиграли " << (-delta) << ". Всего: " << player_.money() << ".\n";
        }
        else {
            cout << "Итог: без изменений. Всего: " << player_.money() << ".\n";
        }
    }

    void playRound(long long bet) {
        initialDealBasic();

        playPlayerHands();

        playDealerBasic();

        cout << "\n--- Итог раунда ---\n";
        showTableState(true);
        cout << "Очки дилера: " << dealer_.hand().score() << "\n";
        for (size_t i = 0; i < player_.hands().size(); ++i) {
            cout << "Очки игрока (рука " << (i + 1) << "): ";
            if (player_.isBusted(i)) cout << "перебор\n";
            else cout << player_.hands()[i].score() << "\n";
        }
        settleBet(bet);
    }

private:
    std::mt19937 rng_;
    Shoe shoe_;
    Player player_;
    Dealer dealer_;
};

// -------------------- main --------------------
int main() {
#ifdef _WIN32 // чтобы ♥♦♣♠ отображались нормально
    SetConsoleOutputCP(CP_UTF8);
    SetConsoleCP(CP_UTF8);
#endif

    setlocale(LC_ALL, "ru-RU");
    try {
        Game g;
        g.run();
    }
    catch (const std::exception& ex) {
        std::cerr << "Fatal error: " << ex.what() << "\n";
        return 1;
    }
    return 0;
}
