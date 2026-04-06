
//TODO: Combine the two below functions via a data-driven definition instead of hard coding
function nameContents(contents) {
    let customString = "" + contents
    switch (customString) {
        case "grass":
            return "/images/grass.png"
        case "water":
            return "/images/water.png"
        case "desert":
            return "/images/desert.png"
        case "mountains":
            return "/images/mountains.png"
        case "fogowar":
            return "/images/fogowar.png"
        case "hills":
            return "/images/hills.png"
        case "trees":
            return "/images/trees.png"
        case "buildings":
            return "/images/buildings.png"
        case "rocky":
            return "/images/rocky.png"
        case "swamp":
            return "/images/swamp.png"
        default:
            console.log("Defaulted when looking at " + contents);
            return "/images/unknown.png"
    }


}

function scale_of(contents) {
    let customString = "" + contents
    switch (customString) {
        case "grass":
            return 1
        case "water":
            return 1
        case "desert":
            return 1
        case "mountains":
            return 0.8
        case "fogowar":
            return 1
        case "hills":
            return 0.8
        case "trees":
            return 0.8
        case "buildings":
            return 0.8
        case "rocky":
            return 1
        case "swamp":
            return 1
        default:
            console.error.log("Defaulted when looking at " + contents);
            return 1
    }


}