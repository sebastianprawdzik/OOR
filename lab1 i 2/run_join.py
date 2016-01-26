import time
import datetime
import threading
import Queue

def log(message):
    now = datetime.datetime.now().strftime("%H:%M:%S")
    print "%s %s" % (now, message)

def oblicz(x):
    time.sleep(x)
    return x * x

# Watki w puli oczekujace na zadania w kolejce ``kolejka_zadan``
class WatekOblicz(threading.Thread):
    def __init__(self, id, kolejka_zadan):
        threading.Thread.__init__(self, name="WatekOblicz-%d" % (id,))
        self.kolejka_zadan = kolejka_zadan
    def run(self):
        while True:
            # watek sie blokuje w oczekiwaniu az cos trafi do kolejki
            req = self.kolejka_zadan.get()
            if req is None:
                # Nie ma nic wiecej do przetwarzania, wiec konczymy
                self.kolejka_zadan.task_done()
                break
            value, kolejka_rezultatow = req
            result = oblicz(value)
            log("%s %s -> %s" % (self.getName(), value, result))
            kolejka_rezultatow.put(result)
            self.kolejka_zadan.task_done()

kolejka_zadan = Queue.Queue()

def threaded_sum(values):
    nsum = 0.0
    kolejka_rezultatow = Queue.Queue()
    for value in values:
        kolejka_zadan.put((value, kolejka_rezultatow))
    # pobranie wynikow
    for _ in values:
        nsum += kolejka_rezultatow.get()
    return nsum

def main():
    log("start watku glownego")
    # ilosc watkow do obliczenia
    N_liczba_watkow = 3
    for i in range(N_liczba_watkow):
        WatekOblicz(i, kolejka_zadan).start()

    # zadania watkow
    result = threaded_sum( (6, 2, 4, 3, 5) )
    log("suma obliczonych watkow wynosi: %f" % (result,))

    # wysylamy zadania zakonczenia przetwarzania do wszystkich watkow
    for i in range(N_liczba_watkow):
        kolejka_zadan.put(None)
    kolejka_zadan.join()
    log("zakonczenie watku glownego")

if __name__ == "__main__":
    main()
