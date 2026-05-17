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
    echo "Usage: scripts/break-production.sh [all|1|2|3|4|5]..."
    echo
    echo "Applies small production-code mutations so the kata tests fail."
    echo "With no arguments, applies all mutations."
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
    selected=("${all_patches[@]}")
else
    for selector in "$@"; do
        if [ "$selector" = "all" ]; then
            selected=("${all_patches[@]}")
            break
        fi

        selected+=("$(patch_for "$selector")")
    done
fi

for patch in "${selected[@]}"; do
    git apply --check "$patch"
done

for patch in "${selected[@]}"; do
    git apply "$patch"
    echo "Applied $patch"
done
