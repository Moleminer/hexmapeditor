function get_hex_corner(center, size, i){
    var angle_deg = 60 * i;
    var angle_rad = Math.PI / 180 * angle_deg;
    return [center.x + size * Math.cos(angle_rad),
                 center.y + size * Math.sin(angle_rad)];
}

// hex_rad is the outer radius that touches the vertices, not inner that touches middles of edges. 
function gen_hex_list(height, width, hex_rad) {
	var hex_height = hex_rad * Math.sqrt(3);
	var hex_width = 2 * hex_rad;

	// Starting values of X and Y
	var current_x = hex_rad;
	var current_y = hex_height / 2;


	var hex_grid = [];
	var isEvenRow = true;
	while (current_y < height) {
		if (isEvenRow) {
			current_x = hex_rad;
		} else {
			current_x = hex_width + (hex_rad/2);
		}
		isEvenRow = !isEvenRow;
		while (current_x < width) {
			const new_hex = {
				x: current_x,
				y: current_y,
				radius: hex_rad
			};
			hex_grid.push(new_hex);
			current_x += hex_width + hex_rad;
		}
		current_y += hex_height / 2;
	}
	return hex_grid;
	

	// get next hex along, get all vertices
	// next row down, only get bottom three vertices and repeat
	// listContainer.innerHTML = listItemsHTML;

}

function hexToString(hex) {
	console.log(hex.x + ", " + hex.y);
}

// Could have target be passed in for better abstraction
function drawHex(hex) {
	const target = document.getElementById("hexDisplay");
	const child = document.createElementNS('http://www.w3.org/2000/svg','polygon');
	var payload = ""
	for (let i = 0; i < 6; i++) {
		var point = get_hex_corner(hex, hex.radius, i);
		console.log("x and y are " + Math.round(point[0]) + " " + Math.round(point[1]))
		payload += Math.round(point[0]) + "," + Math.round(point[1]) + " "
	}
	child.setAttribute("points", payload.trim());
	console.log(payload.trimEnd())
	target.appendChild(child);
}

const debug = document.getElementById("debug");
const viewbox = document.getElementById("hexDisplay");
var hex_list = gen_hex_list(150, 150, 20);
hex_list.forEach(hexToString);
hex_list.forEach(drawHex)


//<script>
//    function drawHexagon() {
//        const view = document.getElementById("hexDisplay");
//        const ctx = canvas.getContext("2d");
//        @* ctx.fillRect(25, 25, 100, 100); *@
//        ctx.beginPath();
//        ctx.moveTo(75, 50);
//        ctx.lineTo(100, 75);
//        ctx.lineTo(100, 25);
//        ctx.lineTo(75, 50);
//        ctx.stroke();
//    }
//    drawHexagon();
//    </script>