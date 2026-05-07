function glazeWithInteract(hex_empty_list) {
    const target = document.getElementById("hexDisplay");
    for (let i = 0; i < hex_empty_list.length; i++) {
        for (let j = 0; j < hex_empty_list[i].length; j++) {
            //After visual layers, add an interaction layer, if we're in edit mode. 
            const glazeLayer = document.createElementNS('http://www.w3.org/2000/svg','polygon');
            payload = "";
            for (let k = 0; k < 6; k++) {
                var point = get_hex_corner(hex_empty_list[i][j], hex_empty_list[i][j].radius, k);
                let x = Math.round(point[0])
                let y = Math.round(point[1])
                payload += x + "," + y + " "
            } 
        glazeLayer.setAttribute("points", payload.trim());
        // glazeLayer.setAttribute("fill", "rgba(255, 0, 0, 0.2)");
		glazeLayer.setAttribute("fill", "none");
        glazeLayer.setAttribute("pointer-events", "all");
		glazeLayer.addEventListener("click", () => {
                    registerClick(this, i, j)
                });
        
        target.appendChild(glazeLayer);
        }
    }
    
}

function registerClick(clickEvent, x, y) {
	const isAdmin = (sessionStorage.getItem("isadmin") == "true");
	if (isAdmin) {
		applyBrushToCell(x, y);
	} else {
		openNote(x, y);
	}
}

function applyBrushToCell(x, y) {
	/** @type {string} */
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

function openNote(x, y) {
	// Very first, we need to know if this cell exists or not. 
	let items = JSON.parse(sessionStorage.getItem("hexmap"));
	let cell = findCell(x, y, items);

	if (cell == null) {
		console.error("Cell does not exist.");
		return;
	}

	// Now that we know the cell exists:

	// Set overlay divs to be visible
	document.getElementById("overlay").style.display = "block";
	document.getElementById("overlay-window").style.display = "block";

	// Update our input to the cell's current description
	const note = document.getElementById('noteinput');
	note.value = cell["Description"]

	const button = document.getElementById("noteinput-button");
	button.removeEventListener("click", () => {});
	button.addEventListener("click", () => {
		console.log("Button pressed")
		// Input name is noteinput
		var hexmap = sessionStorage.getItem("hexmap");
		const form = document.createElement('form');
		
		const input = document.getElementById('noteinput');
		form.id = "noteinputform";
		form.method = "POST";
		form.action  = "/map/updateNote";
		form.append(input);

		const input_x = document.createElement('input');
		input_x.setAttribute("id", "x");
		input_x.name = "x";
		input_x.type = "number";
		input_x.value = x;
		form.append(input_x);

		const input_y = document.createElement('input');
		input_y.setAttribute("id", "y");
		input_y.name = "y";
		input_y.type = "number";
		input_y.value = y;
		form.append(input_y);

		document.body.append(form);
		form.submit();

		// var hexmap = sessionStorage.getItem("hexmap");
		// const form = document.createElement('form');
		// form.id = "transferForm";
		// const input = document.createElement('input');
		// form.method = "POST";
		// form.action  = "/updateNote";
		// input.value = hexmap;
		// input.type = "hidden";
		// input.name = "transferForm";
		// form.append(input);
		// document.body.append(form);
		// form.submit();
	})
}

function closeNote() {
	document.getElementById("overlay").style.display = "none";
	document.getElementById("overlay-window").style.display = "none";
}

function eraseAt(x, y) {
	const hex_list = JSON.parse(sessionStorage.getItem("hexlist"));
	let items = JSON.parse(sessionStorage.getItem("hexmap"));

	// Create the json node to add to items
	const editedCell = findCell(x, y, items);
	if (editedCell != null) {
		editedCell.Values.pop();
		console.log("Found hex, removing top layer")
	} else {
		console.log("Hex was empty, nothing to delete");
		return;
	}
	
	// Redraw layer and add back interact layer
	drawLayer(editedCell, hex_list);
	glazeLayerWithInteract(editedCell, hex_list);

	sessionStorage.setItem("hexmap", JSON.stringify(items));
}

function drawLayer(layer, hex_list) {
	// First, pull assets from session storage to get our single layer from it.
    const assets = JSON.parse(sessionStorage.getItem("assets"));

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
			const scale = getScale(layer.Values[k], assets);
			// Calculate required horizontal offset. A scale of '1' results in an offset of 0. This should work with scales >1 too. 
			const offset = radius * (1 - scale);

			const contentLayer = document.createElementNS('http://www.w3.org/2000/svg','image');

			// Create image to be clipped, and adjust its position based on if it's a foreground or background image. 

			//First, we find the path to the asset
			
			contentLayer.setAttribute("href", getPath(layer.Values[k], assets));

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
function getPath(contents, assets) {
	const isAdmin = sessionStorage.getItem("isadmin");

	var adminSuffix = "";
	var found = false;
	assets.forEach((element) => {
		if (element["Filename"] == contents) {
			found = true;
			if (element["HasAdminView"] == true) {
				adminSuffix = "_admin";
			}
		}
	});
	if (found == false) {
		console.error(`Couldn't find asset for ${contents}. Letting it check image folder anyway.`);
	}
	return `/images/${contents}${adminSuffix}.png`;

}

function getScale(contents, assets) {
	var scale = 1.0;
	var found = false;
	assets.forEach((element) => {
		if (element["Filename"] == contents) {
			scale = element["Scale"];
			found = true;
		}
	});
	if (found == false) {
		console.error(`Couldn't find scale for ${contents}. Defaulting to 1.`);
	}
	
	return scale;


}

/**
 * @param {number} x
 * @param {number} y
 * @param {json} items
 * @returns {json}
 * @description Linearly sorts through given items to find cell. 
* 
* Returns null if search fails, json cell object on success
*/
function findCell(x, y, items) {
	let cell = null;
	for (let i = 0; i < items.length; i++) {
		let hex = items[i];
		if (hex.X == x && hex.Y == y) {
			cell = hex;
			break;
		}
	} 
	return cell;
}

function stopPropogation(event) {
	event.stopPropagation();
}