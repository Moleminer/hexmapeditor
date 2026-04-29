function glazeWithInteract(hex_list) {
    const target = document.getElementById("hexDisplay");
    for (let i = 0; i < hex_list.length; i++) {
        for (let j = 0; j < hex_list[i].length; j++) {
            //After visual layers, add an interaction layer, if we're in edit mode. 
            const glazeLayer = document.createElementNS('http://www.w3.org/2000/svg','polygon');
            payload = "";
            for (let k = 0; k < 6; k++) {
                var point = get_hex_corner(hex_list[i][j], hex_list[i][j].radius, k);
                let x = Math.round(point[0])
                let y = Math.round(point[1])
                payload += x + "," + y + " "
            } 
        glazeLayer.setAttribute("points", payload.trim());
        // glazeLayer.setAttribute("fill", "rgba(255, 0, 0, 0.2)");
		glazeLayer.setAttribute("fill", "none");
        glazeLayer.setAttribute("pointer-events", "all");
        glazeLayer.setAttribute("onclick", `registerClick(this, ${i}, ${j})`);
        
        target.appendChild(glazeLayer);
        }
    }
    
}

function registerClick(clickEvent, x, y) {
	const brush = sessionStorage.getItem("brush");

	// Instead of drawing a layer, erase one if in erase mode
	if (brush == "erase") {
		eraseAt(x, y);
		return;}
	const hex_list = JSON.parse(sessionStorage.getItem("hexlist"));
	const needs_glaze = sessionStorage.getItem("isadmin");
	let items = JSON.parse(sessionStorage.getItem("hexmap"));

	// Create the json node to add to items
	const newLayer = JSON.parse(`{"X":${x},"Y":${y},"Values":["${brush}"]}`)
	let isFound = false;
	for (let i = 0; i < items.length; i++) {
		let hex = items[i];
		// console.log(`Looking at ${hex.X} vs ${x} and ${hex.Y} vs ${y}. Full object being examined is ${JSON.stringify(hex)} `);
		if (hex.X == x && hex.Y == y) {
			isFound = true;
			items[i].Values.push(brush);
			console.log("Found hex, appending")
			break;
		}
	} 
	if (isFound == false) {
		console.log("Hex was empty, starting fresh");
		items.push(newLayer)
	}
	drawLayer(newLayer, hex_list);
	// Only draw interact layer if in editing mode. 
	if (needs_glaze) {
		glazeLayerWithInteract(newLayer, hex_list);
	}
	sessionStorage.setItem("hexmap", JSON.stringify(items));


}

function eraseAt(x, y) {
	const hex_list = JSON.parse(sessionStorage.getItem("hexlist"));
	const needs_glaze = sessionStorage.getItem("isadmin");
	let items = JSON.parse(sessionStorage.getItem("hexmap"));

	// Create the json node to add to items
	
	let isFound = false;
	let editedLayer = null;
	for (let i = 0; i < items.length; i++) {
		let hex = items[i];
		// console.log(`Looking at ${hex.X} vs ${x} and ${hex.Y} vs ${y}. Full object being examined is ${JSON.stringify(hex)} `);
		if (hex.X == x && hex.Y == y) {
			isFound = true;
			editedLayer = items[i];
			let index = editedLayer.Values.indexOf("fogowar")
			if (index > -1) {
				items[i].Values.splice(index, 1);
				editedLayer.Values.splice(index, 1);
			}
			
			console.log("Found hex, removing")
			break;
		}
	} 
	if (isFound == false) {
		console.log("Hex was empty, nothing to delete");
		return;
	}
	drawLayer(editedLayer, hex_list);
	// Only draw interact layer if in editing mode. 
	if (needs_glaze) {
		glazeLayerWithInteract(editedLayer, hex_list);
	}
	sessionStorage.setItem("hexmap", JSON.stringify(items));
}

function drawLayer(layer, hex_list) {
    const radius = hex_list[0][0].radius
	const contents_x = layer.X;
	const contents_y = layer.Y;
	const id = "clip-" + contents_x + "-" + contents_y;

	// Grab our elements we're appending: a hex, and its clip list for cropping
	const target = document.getElementById("hexDisplay");
	const defs = document.getElementById("clipDefs");
	const clipElement = document.createElementNS('http://www.w3.org/2000/svg','clipPath');
	const child = document.createElementNS('http://www.w3.org/2000/svg','polygon');


	// Create polygon for clipping
	var payload = ""
	let MinX = 0;
	let MinY = 0;
	for (let k = 0; k < 6; k++) {
		var point = get_hex_corner(hex_list[contents_x][contents_y], radius, k);
		let x = Math.round(point[0])
		let y = Math.round(point[1])
		payload += x + "," + y + " "

		// Since the hex_corner function runs in the same cycle every time, the lowest X and Y 
		// are always at the same corners of the hexagon being drawn. 
		if (k == 3) MinX = x;
		if (k == 4) MinY = y;
	} 
	child.setAttribute("points", payload.trim());
	clipElement.appendChild(child);
	clipElement.setAttribute("id", id);
	defs.appendChild(clipElement);
	
	for (let k = 0; k < layer.Values.length; k++) {
			// Check scale of the next drawing
			const scale = scale_of(layer.Values[k]);
			// Calculate required horizontal offset. A scale of '1' results in an offset of 0. This should work with scales >1 too. 
			const offset = radius * (1 - scale);

			const contentLayer = document.createElementNS('http://www.w3.org/2000/svg','image');
			// Create image to be clipped, and adjust its position based on if it's a foreground or background image. 
			contentLayer.setAttribute("href", nameContents(layer.Values[k]));
			contentLayer.setAttribute("clip-path", "url(#" + id + ")");
			contentLayer.setAttribute("x", (MinX + offset).toString());
			contentLayer.setAttribute("y", MinY.toString());
			contentLayer.setAttribute("width", radius * scale * 2);
			contentLayer.setAttribute("height", radius * scale * 2);
			target.appendChild(contentLayer);
		
	}
	
}

function glazeLayerWithInteract(layer, hex_list) {
	const layer_x = layer.X;
	const layer_y = layer.Y;
    const target = document.getElementById("hexDisplay");

	//Add an interaction layer to a single edited tile. 
	const glazeLayer = document.createElementNS('http://www.w3.org/2000/svg','polygon');
	payload = "";
	for (let k = 0; k < 6; k++) {
		var point = get_hex_corner(hex_list[layer.X][layer.Y], hex_list[layer.X][layer.Y].radius, k);
		let x = Math.round(point[0])
		let y = Math.round(point[1])
		payload += x + "," + y + " "
	} 
	glazeLayer.setAttribute("points", payload.trim());
	// glazeLayer.setAttribute("fill", "rgba(255, 0, 0, 0.2)");
	glazeLayer.setAttribute("fill", "none");
	glazeLayer.setAttribute("pointer-events", "all");
	glazeLayer.setAttribute("onclick", `registerClick(this, ${layer.X}, ${layer.Y})`);
	
	target.appendChild(glazeLayer);
}

//TODO: Combine the two below functions via a data-driven definition instead of hard coding
function nameContents(contents) {
	const isAdmin = sessionStorage.getItem("isadmin");
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
			if (isAdmin == true) {
				return "/images/border.png"
			} else {
				return "/images/fogowar.png"
			}
            
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
            return 1;
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