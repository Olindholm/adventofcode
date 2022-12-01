
from pathlib import Path
from typing import List


puzzle_input = Path(__file__).parent / "puzzle_input.txt"


def get_puzzle_input() -> List[str]:
    with puzzle_input.open("r") as f:
        return f.readlines()


class Elf:

    def __init__(self) -> None:
        self.items_calories: List[int] = []
    
    def get_calories(self) -> None:
        return sum(self.items_calories)

def main() -> None:
    input = get_puzzle_input()
    elf_inputs = "".join(input).split("\n\n")

    elves: List[Elf] = []
    for elf_input in elf_inputs:
        elf_items = elf_input.strip().split("\n")
        elf = Elf()
        for elf_item in elf_items:
            elf.items_calories.append(int(elf_item))
        
        elves.append(elf)

    fattest_elf = max(elves, key=lambda elf: elf.get_calories())
    print(fattest_elf.get_calories())
    


if __name__ == "__main__":
    main()

