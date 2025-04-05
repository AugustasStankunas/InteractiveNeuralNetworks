import torch


def log():
    with open("log.txt", "w") as file:
        file.write(torch.__version__)
        file.write("\n")
        file.write(str(torch.cuda.is_available()))


def main():
    log()


if __name__ == "__main__":
    main()