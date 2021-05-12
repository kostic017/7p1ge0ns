void main() {
    n = 0
    rev = 0
    prompt("n: ", n)

    while (n != 0) {
        rev *= 10 + n % 10
        n /= 10
    }

    print("rev = " + rev)
}