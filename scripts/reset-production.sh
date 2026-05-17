#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "${BASH_SOURCE[0]}")/.."

patch_1="scripts/mutations/01-allow-duplicate-shirt-numbers.patch"
patch_2="scripts/mutations/02-add-manuel-neuer.patch"
patch_3="scripts/mutations/03-remove-united-states-goalkeeper.patch"
patch_4="scripts/mutations/04-invert-game-points.patch"
patch_5="scripts/mutations/05-allow-points-for-team-that-did-not-play.patch"
all_patches=("$patch_1" "$patch_2" "$patch_3" "$patch_4" "$patch_5")

usage() {
    echo "Usage: scripts/reset-production.sh [all|1|2|3|4|5]..."
    echo
    echo "Reverts production-code mutations applied by break-production.sh."
    echo "With no arguments, reverts the mutations that are currently applied."
}

is_applied() {
    git apply -R --check "$1" >/dev/null 2>&1
}

patch_for() {
    case "$1" in
        1) echo "$patch_1" ;;
        2) echo "$patch_2" ;;
        3) echo "$patch_3" ;;
        4) echo "$patch_4" ;;
        5) echo "$patch_5" ;;
        *)
            usage >&2
            exit 1
            ;;
    esac
}

selected=()

if [ "$#" -eq 0 ]; then
    for patch in "${all_patches[@]}"; do
        if is_applied "$patch"; then
            selected+=("$patch")
        fi
    done

    if [ "${#selected[@]}" -eq 0 ]; then
        echo "No production-code mutations are currently applied."
        exit 0
    fi
else
    for selector in "$@"; do
        if [ "$selector" = "all" ]; then
            selected=("${all_patches[@]}")
            break
        fi

        selected+=("$(patch_for "$selector")")
    done
fi

for ((index=${#selected[@]} - 1; index >= 0; index--)); do
    if ! is_applied "${selected[$index]}"; then
        echo "Cannot reset ${selected[$index]} because that mutation is not currently applied." >&2
        echo "Run scripts/reset-production.sh with no arguments to reset only applied mutations." >&2
        exit 1
    fi
done

for ((index=${#selected[@]} - 1; index >= 0; index--)); do
    git apply -R "${selected[$index]}"
    echo "Reverted ${selected[$index]}"
done
