use std::{collections::BTreeMap, num::TryFromIntError, ops::AddAssign};

const INPUT: &str = include_str!("input.txt");

fn main() {
    println!("Task 01: {}", task_01(INPUT));
    println!("Task 02: {}", task_02(INPUT));
}

#[derive(Debug)]
struct Node {
    value: char,
    column: usize,
    row: usize
}

fn task_01(input: &str) -> usize {
    const POSSIBLE_DIRECTIONS: [(i32, i32); 8] = [
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, 1),
        (1, 1),
        (1, 0),
        (1, -1),
        (0, -1)
    ];

    let number_of_columns = input.lines().next().unwrap().len();
    let all_nodes: Vec<Node> = input.chars()
        .filter(|c| !c.is_whitespace())
        .enumerate()
        .map(|(i, c)| Node {
            value: c,
            column: i % number_of_columns,
            row: i / number_of_columns
        })
        .collect();
    let map = get_map_from_nodes(&all_nodes);

    let entry_points = all_nodes
        .iter()
        .filter(|node| node.value == 'X');

    let mut result = 0;
    for entry_point in entry_points {
        result.add_assign(get_all_permutations(entry_point, "XMAS", &map, &POSSIBLE_DIRECTIONS).into_iter().count());
    }
    result
}

fn task_02(input: &str) -> usize {
    const POSSIBLE_DIRECTIONS: [(i32, i32); 4] = [
        (-1, -1),
        (-1, 1),
        (1, 1),
        (1, -1)
    ];

    let number_of_columns = input.lines().next().unwrap().len();
    let all_nodes: Vec<Node> = input.chars()
        .filter(|c| !c.is_whitespace())
        .enumerate()
        .map(|(i, c)| Node {
            value: c,
            column: i % number_of_columns,
            row: i / number_of_columns
        })
        .collect();
    let map = get_map_from_nodes(&all_nodes);

    let entry_points = all_nodes
        .iter()
        .filter(|node| node.value == 'M');

    let mut centers: BTreeMap<(usize, usize), i32> = BTreeMap::new();
    for entry_point in entry_points {
        let centers_of_permutation: Vec<&Node> = get_all_permutations(entry_point, "MAS", &map, &POSSIBLE_DIRECTIONS)
            .iter()
            .map(|permutation| permutation.iter().filter(|node| node.value == 'A').map(|node| *node).next().unwrap())
            .collect();

        for center_of_permutation in centers_of_permutation {
            match centers.get_mut(&(center_of_permutation.column, center_of_permutation.row)) {
                Some(reference) => {
                    reference.add_assign(1);
                },
                None => {
                    centers.insert((center_of_permutation.column, center_of_permutation.row), 1);
                }
            }
        }
    }

    centers.iter()
        .filter(|(_, v)| **v == 2)
        .count()
}

fn get_map_from_nodes(nodes: &Vec<Node>) -> BTreeMap<(usize, usize), &Node> {
    let mut map = BTreeMap::new();
    for node in nodes {
        map.insert((node.column, node.row), node);
    }
    return map;
}

fn get_all_permutations<'a>(node: &'a Node, target: &'a str, map: &'a BTreeMap<(usize, usize), &'a Node>, possible_directions: &'a [(i32, i32)]) -> Vec<Vec<&'a Node>> {
    let mut result = Vec::with_capacity(possible_directions.len());

    for (dx, dy) in possible_directions {
        let mut point = node;
        let mut valid = true;
        let mut point_valid = true;
        let mut permutation = Vec::with_capacity(target.len());
        permutation.push(point);

        for c in target.chars() {
            if point.value != c {
                valid = false;
                break;
            }

            if !point_valid {
                valid = false;
                break;
            }

            
            let new_column: Result<usize, TryFromIntError> = ((point.column as i32) + dx).try_into();
            let new_row: Result<usize, TryFromIntError> = ((point.row as i32) + dy).try_into();

            if new_column.is_err() || new_row.is_err() {
                point_valid = false;
                continue;
            }

            match map.get(&(new_column.unwrap(), new_row.unwrap())) {
                Some(new_point) => {
                    point = *new_point;
                    permutation.push(point);
                },
                None => {
                    point_valid = false;
                    continue;
                }
            }
        }

        if valid {
            result.push(permutation);
        }
    }

    return result;
}

#[cfg(test)]
mod tests {
    use super::*;

    const EXAMPLE_INPUT: &str = "MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
";

    const SMALL_EXAMPLE_INPUT: &str = "..X...
.SAMX.
.A..A.
XMAS.S
.X....
";

    #[test]
    fn task_01_small_example() {
        assert_eq!(task_01(SMALL_EXAMPLE_INPUT), 4);
    }

    #[test]
    fn task_01_example() {
        assert_eq!(task_01(EXAMPLE_INPUT), 18);
    }

    #[test]
    fn task_01_solution() {
        assert_eq!(task_01(INPUT), 2554);
    }

    #[test]
    fn task_02_example() {
        assert_eq!(task_02(EXAMPLE_INPUT), 9);
    }

    #[test]
    fn task_02_solution() {
        assert_eq!(task_02(INPUT), 1916);
    }
}