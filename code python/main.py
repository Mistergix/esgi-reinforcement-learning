import random


def dropCoin(n, coin):
    coin -= 1
    if n == 1:
        if random.random() < 0.05:
            coin += 2
    elif n == 2:
        if random.random() < 0.3:
            coin += 2
    else:
        if random.random() < 0.6:
            coin += 2
    return coin

def selectCoin(epsilon):
    p = random.random()

    if p < epsilon:
        return random.randint(1, 3)
    else:
        if pc1 > pc2 and pc1 > pc3:
            return 1
        elif pc2 > pc1 and pc2 > pc3:
            return 2
        elif pc3 > pc1 and pc3 > pc2:
            return 3
        else:
            return random.randint(1, 3)


epsilon = 0.2
coin = 100

nc1 = 0
nc2 = 0
nc3 = 0
wc1 = 0
wc2 = 0
wc3 = 0

pc1 = 0
pc2 = 0
pc3 = 0

n = 0
while coin > 0 and coin < 200:
    n += 1
    c = selectCoin(epsilon)
    r = (1000 - n)/1000
    epsilon = (0.3 - 0) * r + 0
    bcoin = coin
    coin = dropCoin(c, coin)
    win = bcoin < coin
    if c == 1:
        nc1 += 1
        if win:
            wc1 +=1
        pc1 = wc1 / nc1
    if c == 2:
        nc2 += 1
        if win:
            wc2 +=1
        pc2 = wc2 / nc2
    if c == 3:
        nc3 += 1
        if win:
            wc3 +=1
        pc3 = wc3 / nc3


    print("p1:" + str(pc1))
    print("p2:" + str(pc2))
    print("p3:" + str(pc3))

    print("epsilon:" + str(epsilon))

    print("coin: " + str(coin))
    print("selected coin " + str(c))
    print("win:" + str(win))



