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
        glazeLayer.setAttribute("fill", "rgba(255, 0, 0, 0.2)");
        glazeLayer.setAttribute("pointer-events", "all");
        glazeLayer.setAttribute("onclick", `registerClick(this, ${i}, ${j})`);
        
        target.appendChild(glazeLayer);
        }
    }
    
}
