bool prime(int n) {
    if (n == 1)
        return false

    i = 2
    while (i <= n / 2) {
        if (n % i == 0)
            return false
        i = i + 1
    }

    return true
}

void main() {
    n = 0
    prompt("n: ", n)

    if prime(n)
        print("prime")
    else
        print("composite")
}