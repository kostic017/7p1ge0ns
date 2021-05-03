const int N_1 = 4;
const int N_2 = 5;

int max(int a, int b) {
    return a > b ? a : b;
}

void main() {
    print(1 + 2 - 4 / 5 % 6 * 7 ^ 2);

    if max(N_1, N_2) == N_1 {
        print("N_1");
    } else {
        print("N_2");
    }
}