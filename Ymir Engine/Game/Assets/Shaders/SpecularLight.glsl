#version 450 core

#ifdef VERTEX_SHADER

	layout(location = 0) in vec3 aPos;
	layout(location = 1) in vec3 aNormal;
	layout(location = 2) in vec2 aTexCoords;

	layout(location = 5) in vec3 aTangents;
	layout(location = 6) in vec3 aBitangents;
	
    struct PointLight {    
        vec3 lightDir;
        float lightInt;
	    vec3 lightColor;
    };  

    struct TangentPointLight {    
        vec3 TangentLightPos;
        vec3 lightColor;
        float lightInt;
    }; 

    #define MAX_POINT_LIGHTS 4

    out mat3 TBN;
	out vec3 Normal;
	out vec2 TexCoords;

    out TangentPointLight tPointLights[MAX_POINT_LIGHTS];
    out vec3 TangentViewPos;
    out vec3 TangentFragPos;
	
	uniform mat4 projection;
	uniform mat4 view;
	uniform mat4 model;
	
	uniform vec3 camPos;

    uniform int numPointLights;
    uniform PointLight pointLights[MAX_POINT_LIGHTS];

	void main()
	{
		gl_Position = projection * view * model * vec4(aPos, 1.0f);

		vec3 Position  = (model * vec4(aPos, 1.0f)).xyz;
		Normal = aNormal;
		TexCoords = aTexCoords;
		
		vec3 T = normalize(vec3(model * vec4(aTangents,   0.0)));
   		vec3 B = normalize(vec3(model * vec4(aBitangents, 0.0)));
   		vec3 N = normalize(vec3(model * vec4(aNormal,    0.0)));
   		TBN = mat3(T, B, N);
   		TBN = transpose(TBN);
   		
        TangentViewPos = TBN * camPos;
    	TangentFragPos = TBN * Position;

        for (int i = 0; i < numPointLights; i++) {

            tPointLights[i].TangentLightPos = TBN * pointLights[i].lightDir;
            tPointLights[i].lightInt = pointLights[i].lightInt;
            tPointLights[i].lightColor =  pointLights[i].lightColor;

        }
   		
	}

#endif

#ifdef FRAGMENT_SHADER
	
    // Point Lights

    struct TangentPointLight {    
        vec3 TangentLightPos;
        vec3 lightColor;
        float lightInt;
    }; 
    
    #define MAX_POINT_LIGHTS 4
    in TangentPointLight tPointLights[MAX_POINT_LIGHTS];
    uniform int numPointLights;

    uniform int numLights;

    uniform int map;
    
	in vec3 Normal;
	in vec2 TexCoords;
	
    in vec3 TangentViewPos;
    in vec3 TangentFragPos;

    in mat3 TBN;

    out vec4 FragColor;

    uniform sampler2D texture_diffuse;
	uniform sampler2D texture_specular;
	uniform sampler2D texture_normal;
	uniform sampler2D texture_height;
	uniform sampler2D texture_ambient;
	uniform sampler2D texture_emissive;
    
   	uniform bool enableDiffuse;
    uniform bool enableSpecular;
    uniform bool enableNormal;
    uniform bool enableAmbient;
    uniform bool enableHeight;
    uniform bool enableEmissive;
    
    uniform bool enableLighting;

   	uniform float transparency = 1.0f;
   	uniform float ambient = 0.50f;
    uniform float specularLight = 0.50f;
    uniform float normalIntensity = 1.0f;
	uniform float emissive = 1.0f;
	
	vec4 CalculatePointLight(TangentPointLight light, vec3 normal, vec3 TangentFragPos, vec3 viewDirection) {
		
        // Point Light Attenuation

		vec3 lightVec = light.TangentLightPos - TangentFragPos;
		float dist = length(lightVec);
		float a = 0.005;
		float b = 0.0001;
		float intensity = 1.0f / (a * dist * dist + b * dist + 1.0f);
            
        // Normal

        vec3 lightDirection = normalize(light.TangentLightPos - TangentFragPos);
        float diffuse = max(dot(normal, lightDirection), 0.0f);
        
        // Specular
        
        vec3 reflectionDirection = reflect(-lightDirection, normal);
        float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
        float specular = specAmount * specularLight;
		
		// Apply texture maps

        vec3 finalColor = vec3(0.0); // Initialize the final color variable

        // Apply diffuse and ambient texture
        finalColor += (enableDiffuse ? texture(texture_diffuse, TexCoords).rgb : vec3(1.0f)) * light.lightColor * (diffuse * intensity +
        ambient * intensity * (enableAmbient ? texture(texture_ambient, TexCoords).r : 1.0f));

        // Apply specular texture
        if (enableSpecular) finalColor += texture(texture_specular, TexCoords).a * specular * intensity;

        // Apply emissive texture
        if (enableEmissive) finalColor += texture(texture_emissive, TexCoords).rgb * emissive;

        // Apply transparency
        return vec4(finalColor,transparency);
	
	}
	
    vec4 BakePointLight(vec3 lightPos, vec3 lightColor, float lightInt, vec3 normal, vec3 TangentFragPos, vec3 viewDirection) {
		
        lightPos = TBN * lightPos;

        // Point Light Attenuation

		vec3 lightVec = lightPos - TangentFragPos;
		float dist = length(lightVec);
		float a = 0.005;
		float b = 0.0001;
		float intensity = 1.0f / (a * dist * dist + b * dist + 1.0f);
            
        // Normal

        vec3 lightDirection = normalize(lightPos - TangentFragPos);
        float diffuse = max(dot(normal, lightDirection), 0.0f);
        
        // Specular
        
        vec3 reflectionDirection = reflect(-lightDirection, normal);
        float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
        float specular = specAmount * specularLight;
		
		// Apply texture maps

        vec3 finalColor = vec3(0.0); // Initialize the final color variable

        // Apply diffuse and ambient texture
        finalColor += (enableDiffuse ? texture(texture_diffuse, TexCoords).rgb : vec3(1.0f)) * lightColor * (diffuse * intensity +
        ambient * intensity * (enableAmbient ? texture(texture_ambient, TexCoords).r : 1.0f));

        // Apply specular texture
        if (enableSpecular) finalColor += texture(texture_specular, TexCoords).a * specular * intensity;

        // Apply emissive texture
        if (enableEmissive) finalColor += texture(texture_emissive, TexCoords).rgb * emissive;

        // Apply transparency
        return vec4(finalColor,transparency);
	
	}

	vec4 DirectionalLight() {
		
		float ambient = 0.50f;
            
        //vec3 normal = normalize(Normal);
        vec3 normalMap = texture(texture_normal, TexCoords).xyz * 2.0f - 1.0f;
        vec3 normal = normalize(normalMap);
        //vec3 lightDirection = normalize(TangentLightPos);
        vec3 lightDirection = normalize(vec3(0.0f));
            
        float specularLight = 0.50f;
        vec3 viewDirection = normalize(TangentViewPos - TangentFragPos);
        vec3 reflectionDirection = reflect(-lightDirection, normal);
        float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
        float specular = specAmount * specularLight;
            
        float diffuse = max(dot(normal, lightDirection), 0.0f);
         
        return vec4(
            texture(texture_diffuse, TexCoords).rgb * /*light.lightColor **/ (diffuse+ ambient) + 
            texture(texture_specular, TexCoords).r * specular, transparency
        );
	
	}
	
	vec4 SpotLight() {
		
		float outerCone = 0.90f;
		float innerCone = 0.95f;
		
		float ambient = 0.20f;
            
        //vec3 normal = normalize(Normal);
        vec3 normalMap = texture(texture_normal, TexCoords).xyz * 2.0f - 1.0f;
        vec3 normal = normalize(normalMap);
        //vec3 lightDirection = normalize(TangentLightPos - TangentFragPos);
        vec3 lightDirection = normalize(vec3(0.0f));
        
        float diffuse = max(dot(normal, lightDirection), 0.0f);
        
        float specularLight = 0.50f;
        vec3 viewDirection = normalize (TangentViewPos - TangentFragPos);
        vec3 reflectionDirection = reflect(-lightDirection, normal);
        float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
        float specular = specAmount * specularLight;
            
        float angle = dot(vec3(0.0f, -1.0f, 0.0f), -lightDirection);
        float intensity = clamp((angle - outerCone) / (innerCone - outerCone), 0.0f, 1.0f);
         
        return vec4(
            texture(texture_diffuse, TexCoords).rgb * /*light.lightColor **/ (diffuse * intensity + ambient) + 
            texture(texture_specular, TexCoords).r * specular * intensity, transparency
        );
	
	}

    vec4 AreaLight() {

        return vec4(0.0f);

    }

    vec4 CalculateBaseLight(vec3 normal, vec3 TangentFragPos, vec3 viewDirection) {
            
        // Normal

        vec3 lightDirection = normalize(TangentFragPos);
        float diffuse = max(dot(normal, lightDirection), 0.0f);
        
        // Specular
        
        vec3 reflectionDirection = reflect(-lightDirection, normal);
        float specAmount = pow(max(dot(viewDirection, reflectionDirection), 0.0f), 16);
        float specular = specAmount * specularLight;
		
		// Apply texture maps

        vec3 finalColor = vec3(0.0); // Initialize the final color variable

        // Apply diffuse and ambient texture
        finalColor += (enableDiffuse ? texture(texture_diffuse, TexCoords).rgb : vec3(1.0f)) * (diffuse +
        ambient * (enableAmbient ? texture(texture_ambient, TexCoords).r : 1.0f));

        // Apply specular texture
        if (enableSpecular) finalColor += texture(texture_specular, TexCoords).a * specular;

        // Apply emissive texture
        if (enableEmissive) finalColor += texture(texture_emissive, TexCoords).rgb * emissive;

        // Apply transparency
        return vec4(finalColor,transparency);
    }

    void main()
    {   
        vec3 normalMap = texture(texture_normal, TexCoords).xyz * 2.0f - 1.0f;
        vec3 normal = (enableNormal ? normalize(normalMap) : normalize(Normal)) * normalIntensity;

        vec3 viewDirection = normalize(TangentViewPos - TangentFragPos);

        vec4 finalColor;
		
		if (numPointLights > 0 && enableLighting) // Light Management (we only handle Point Lights for the moment)
        {
            // Directional Light Management

            // Point Light Management
            
	        for (int i = 0; i < numPointLights; i++) 
            {
	            finalColor += CalculatePointLight(tPointLights[i], normal, TangentFragPos, viewDirection);
	        }    

            // Spot Light Management

            // Area Light Management

            // ----------------------- Baked Lights ----------------------- \\

            // Baked Lights BASE
            if (map == 0) {

                finalColor += BakePointLight(vec3(52.6f,24.0f,-5.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylights1
                finalColor += BakePointLight(vec3(22.3f,24.0f,-5.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylights2
                finalColor += BakePointLight(vec3(22.3f,24.0f,11.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylights3
                finalColor += BakePointLight(vec3(52.6f,24.0f,11.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylights4

                finalColor += BakePointLight(vec3(96.0f,15.0f,1.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights1
                //finalColor += BakePointLight(vec3(126.0f,15.0f,1.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights2
                finalColor += BakePointLight(vec3(149.2f,15.0f,1.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights3
                finalColor += BakePointLight(vec3(126.3f,15.0f,-19.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights4
                //finalColor += BakePointLight(vec3(95.7f,15.0f,-19.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights5
                finalColor += BakePointLight(vec3(160.8f,15.0f,-19.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights6
                finalColor += BakePointLight(vec3(182.3f,15.0f,5.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights7
                //finalColor += BakePointLight(vec3(164.3f,15.0f,21.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights8
                finalColor += BakePointLight(vec3(91.0f,15.0f,21.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //upgraderoomlights9

                finalColor += BakePointLight(vec3(126.5f,6.0f,25.4f), vec3(0.0f,1.0f,0.3f), 1, normal, TangentFragPos, viewDirection); //chestlight
                //finalColor += BakePointLight(vec3(-33.5f,5.0f,-0.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tpmonitorlights

                finalColor += BakePointLight(vec3(-15.5f,15.0f,-37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights1
                finalColor += BakePointLight(vec3(-44.5f,15.0f,-37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights2
                finalColor += BakePointLight(vec3(-74.0f,15.0f,-37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights3
                finalColor += BakePointLight(vec3(-118.0f,15.0f,-37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights4
                finalColor += BakePointLight(vec3(-15.5f,15.0f,37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights5
                finalColor += BakePointLight(vec3(-44.5f,15.0f,37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights6
                finalColor += BakePointLight(vec3(-74.0f,15.0f,37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights7
                finalColor += BakePointLight(vec3(-118.0f,15.0f,37.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights8
                finalColor += BakePointLight(vec3(-68.0f,29.0f,0.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tproomlights9
                
                //finalColor += BakePointLight(vec3(-101.0f,19.5f,17.7f), vec3(0.5f,0.5f,1.0f), 1, normal, TangentFragPos, viewDirection); //tpscreenlights1
                //finalColor += BakePointLight(vec3(-101.0f,19.5f,-17.7f), vec3(0.5f,0.5f,1.0f), 1, normal, TangentFragPos, viewDirection); //tpscreenlights2

                finalColor += BakePointLight(vec3(-131.8f,29.0f,0.0f), vec3(0.0f,0.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tplights

            }

            // Baked Lights LVL1
            if (map == 1) {

            finalColor += BakePointLight(vec3(0.0f,27.1f,2.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tutoriallights1
            finalColor += BakePointLight(vec3(42.1f,27.1f,-27.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tutoriallights2
            finalColor += BakePointLight(vec3(82.2f,27.1f,-67.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //tutoriallights3

            finalColor += BakePointLight(vec3(120.1f,27.1f,-121.5f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights1
            finalColor += BakePointLight(vec3(141.6f,27.1f,-92.5f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights2
            finalColor += BakePointLight(vec3(141.6f,27.1f,-133.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights3
            finalColor += BakePointLight(vec3(165.6f,27.1f,-165.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights4
            finalColor += BakePointLight(vec3(193.1f,27.1f,-196.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights5
            finalColor += BakePointLight(vec3(245.1f,27.1f,-215.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights6
            finalColor += BakePointLight(vec3(193.9f,27.1f,-276.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights7
            finalColor += BakePointLight(vec3(112.9f,27.1f,-244.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights8
            finalColor += BakePointLight(vec3(284.3f,27.1f,-215.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights9
            finalColor += BakePointLight(vec3(296.5f,27.1f,-160.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights10
            finalColor += BakePointLight(vec3(302.8f,27.1f,-118.5f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights11
            finalColor += BakePointLight(vec3(241.0f,27.1f,-99.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights12
            finalColor += BakePointLight(vec3(416.7f,27.1f,-92.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights13
            finalColor += BakePointLight(vec3(321.6f,27.1f,-7.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights14
            finalColor += BakePointLight(vec3(450.3f,27.1f,50.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights15
            finalColor += BakePointLight(vec3(450.3f,27.1f,50.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //entrancelights16

            finalColor += BakePointLight(vec3(251.2f,27.1f,56.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights1
            finalColor += BakePointLight(vec3(195.2f,27.1f,103.5f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights2
            finalColor += BakePointLight(vec3(321.4f,27.1f,242.5f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights3
            finalColor += BakePointLight(vec3(504.8f,27.1f,109.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights4
            finalColor += BakePointLight(vec3(137.1f,27.1f,179.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights5
            finalColor += BakePointLight(vec3(629.3f,27.1f,238.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights6
            finalColor += BakePointLight(vec3(569.5f,27.1f,318.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights7
            finalColor += BakePointLight(vec3(635.2f,27.1f,385.7f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights8
            finalColor += BakePointLight(vec3(743.7f,27.1f,215.7f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights9
            finalColor += BakePointLight(vec3(892.8f,27.1f,60.7f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights10
            finalColor += BakePointLight(vec3(938.3f,27.1f,-113.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights11
            finalColor += BakePointLight(vec3(1032.5f,27.1f,-200.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights12
            finalColor += BakePointLight(vec3(934.7f,27.1f,-278.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights13
            finalColor += BakePointLight(vec3(870.7f,27.1f,-192.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights14
            finalColor += BakePointLight(vec3(591.5f,27.1f,-252.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights15
            finalColor += BakePointLight(vec3(1054.2f,27.1f,54.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights16
            finalColor += BakePointLight(vec3(587.5f,27.1f,-254.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //midlights17

            }

            // Baked Lights LVL2_PART_1
            if (map == 2) {
                                                    // Posicion           // Color [0,1]
                finalColor += BakePointLight(vec3(133.5f,108.4f,1.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //reactorlight1
                finalColor += BakePointLight(vec3(-24.3f,108.4f,66.4f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //reactorlight2
                finalColor += BakePointLight(vec3(17.6f,108.4f,2.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //reactorlight3
                finalColor += BakePointLight(vec3(132.8f,108.4f,98.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //reactorlight4

                finalColor += BakePointLight(vec3(-143.7f,108.4f,82.8f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylight1
                finalColor += BakePointLight(vec3(-327.3f,108.4f,42.7f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //hallwaylight2
                finalColor += BakePointLight(vec3(-291.3f,108.4f,131.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //secretroom1
                finalColor += BakePointLight(vec3(-326.9f,108.4f,271.1f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //secretroom2

                finalColor += BakePointLight(vec3(-240.0f,108.4f,294.3f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //monitorroomlights1
                finalColor += BakePointLight(vec3(-239.7f,108.4f,168.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //monitorroomlights2
                finalColor += BakePointLight(vec3(-87.2f,108.4f,126.3f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //monitorroomlights3
                finalColor += BakePointLight(vec3(-51.4f,108.4f,234.2f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //monitorroomlights4
                finalColor += BakePointLight(vec3(-136.6f,108.4f,313.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //monitorroomlights5
                
                finalColor += BakePointLight(vec3(216.2f,108.4f,119.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights1
                finalColor += BakePointLight(vec3(252.4f,108.4f,18.6f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights2
                finalColor += BakePointLight(vec3(216.8f,108.4f,-42.7f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights3
                finalColor += BakePointLight(vec3(251.8f,108.4f,318.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights4
                finalColor += BakePointLight(vec3(146.8f,108.4f,253.3f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights5
                finalColor += BakePointLight(vec3(53.8f,108.4f,294.3f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights6
                finalColor += BakePointLight(vec3(294.1f,108.4f,137.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights7
                finalColor += BakePointLight(vec3(257.0f,108.4f,226.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights8
                finalColor += BakePointLight(vec3(378.0f,108.4f,386.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //abysshwlights9

                finalColor += BakePointLight(vec3(117.9f,108.4f,-137.9f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //intersectionlights1
                finalColor += BakePointLight(vec3(234.0f,108.4f,-318.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //intersectionlights2
                finalColor += BakePointLight(vec3(358.4f,108.4f,-141.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //intersectionlights3
                finalColor += BakePointLight(vec3(236.2f,114.7f,-194.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //intersectionlights4
                finalColor += BakePointLight(vec3(236.2f,120.7f,-194.0f), vec3(1.0f,1.0f,1.0f), 1, normal, TangentFragPos, viewDirection); //intersectionlights5
                finalColor += BakePointLight(vec3(273.7f,108.4f,-77.4f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors1
                finalColor += BakePointLight(vec3(273.7f,108.4f,-106.8f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors2
                finalColor += BakePointLight(vec3(273.7f,108.4f,-140.0f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors3
                finalColor += BakePointLight(vec3(307.2f,108.4f,-140.0f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors4
                finalColor += BakePointLight(vec3(307.2f,108.4f,-107.0f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors5
                finalColor += BakePointLight(vec3(197.4f,108.4f,-77.4f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors6
                finalColor += BakePointLight(vec3(197.4f,108.4f,-106.8f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors7
                finalColor += BakePointLight(vec3(197.4f,108.4f,-140.0f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors8
                finalColor += BakePointLight(vec3(160.1f,108.4f,-140.0f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors9
                finalColor += BakePointLight(vec3(160.1f,108.4f,-106.8f), vec3(0.0f,0.2f,0.7f), 1, normal, TangentFragPos, viewDirection); //monitors8
                finalColor += BakePointLight(vec3(132.4f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers1
                finalColor += BakePointLight(vec3(152.4f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers2
                finalColor += BakePointLight(vec3(169.4f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers3
                finalColor += BakePointLight(vec3(149.4f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers4
                finalColor += BakePointLight(vec3(169.1f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers5
                finalColor += BakePointLight(vec3(186.3f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers6
                finalColor += BakePointLight(vec3(287.2f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers7
                finalColor += BakePointLight(vec3(305.4f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers8
                finalColor += BakePointLight(vec3(323.8f,104.2f,-238.9f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers9
                finalColor += BakePointLight(vec3(308.0f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers10
                finalColor += BakePointLight(vec3(327.2f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers11
                finalColor += BakePointLight(vec3(344.2f,104.2f,-211.3f), vec3(0.0f,0.7f,0.7f), 1, normal, TangentFragPos, viewDirection); //servers12

            }

            // Baked Lights LVL2_PART_2
            if (map == 3) {



            }

            // Baked Lights LVL3_PART_1
            if (map == 4) {



            }

            // Baked Lights LVL3_BOSS
            if (map == 5) {



            }

        }
        else 
        { 
            // Base Light Management
            finalColor += CalculateBaseLight(normal, TangentFragPos, viewDirection);
        }

        FragColor = finalColor;
    }

#endif









