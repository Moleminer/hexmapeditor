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
	var grid_x = 0;
	var grid_y = 0;

	var hex_grid = [];
	var isEvenRow = true;

	while (grid_y < height) {
		var hex_grid_row = [];
		grid_x = 0;
		if (isEvenRow) {
			current_x = hex_rad;
		} else {
			current_x = hex_width + (hex_rad/2);
		}
		isEvenRow = !isEvenRow;
		while (grid_x < width) {
			const new_hex = {
				x: current_x,
				y: current_y,
				radius: hex_rad,
				grid_x: grid_x,
				grid_y: grid_y
			};
			hex_grid_row.push(new_hex);
			current_x += hex_width + hex_rad;
			grid_x += 1;
		}
		hex_grid.push(hex_grid_row);
		current_y += hex_height / 2;
		grid_y += 1;
	}
	return hex_grid; 
	

	// get next hex along, get all vertices
	// next row down, only get bottom three vertices and repeat
	// listContainer.innerHTML = listItemsHTML;

}


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