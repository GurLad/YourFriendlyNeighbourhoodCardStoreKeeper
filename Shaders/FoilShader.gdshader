shader_type canvas_item;

uniform float rate = -0.5f;
uniform float slant = 0.5f;
uniform float alpha = 0.15f;

void vertex() {
	// Called for every vertex the material is visible on.
}

vec3 hsv2rgb(vec3 c)
{
    vec4 K = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void fragment() {
	vec3 hsv = vec3((TIME * rate + UV.y + UV.x * slant), 1, 1);
	float fullAlpha = texture(TEXTURE, UV).a * alpha;
	COLOR = vec4(hsv2rgb(hsv), fullAlpha);
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
