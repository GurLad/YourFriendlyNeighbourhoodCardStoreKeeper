shader_type canvas_item;

uniform float rate = 3;
uniform float maxAdd = 0.2f;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 col = texture(TEXTURE, UV);
	float add = maxAdd * (sin(TIME * rate) + 1.0f) / 2.0f;
	col = vec4(col.r + add, col.g + add, col.b + add, col.a);
	COLOR = col;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
