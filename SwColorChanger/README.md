# How to use

1. Create color mapping file with *.json* extension.
2. If there is a "colors.json" file, it will be used by default. Otherwise, the program will ask you to specify color mappings file (relative path with extension, like *colors1*)
3. If no arguments were passed to program, it will ask you to specify vehicle save name without file extension (like *bmp_v1_aps*, it will try to open "%appdata%/Stormworks/data/vehicles/bmp_v1_aps.xml")
4. If everything was ok, the program will recolor vehicle and save it to *vehiclename*_rc save

## Color mapping files
### Sample mapping file:
```json
{
  "0,94,115": {
    "$randSat": {
      "base": "0,94,115",
      "deviation" : 50
    }
  },
  "FFFFFF": {
    "$randSat": {
      "base": "f1dfd4",
      "deviation" : 50
    }
  },
  "515C4c": {
    "$randSat": {
      "base": "515C4C",
      "deviation": 50
    }
  },
  "255,0,0": "0,255,0",
  "0,255,0": "FFFFFF"
}
```
### Syntax:
JSON object. Keys are colors that will be replaced.
Colors can be provided in following syntaxes:
- RRGGBB (hex, e.g. FF0091)
- R,G,B (decimal from 0 to 255, e.g. 127, 3, 15)

Values can be either constant color or a special macros.
#### Constant color syntax:
`"RRGGBB":"rrggbb"` * Note: You can also use second color syntax: `"RRGGBB":"r,g,b"`

#### Random saturation operator
```json
  "[color to replace]" : {
    "$randSat": {
      "base": "[base color]",
      "deviation": [saturation deviation]
    }
  }
```

#### Random from set operator
```json
{
  "255,255,255": {
    "$randFrom": [
      {
        "//": "Probability will be calculated automatically",
        "$randSat": {
          "base": "227, 224, 232", "//": "SnowColor",
          "deviation": 50
        }
      },
      {
        "probability": 0.25, "//": "Amount of color",
        "$randSat": {
          "base": "49, 63, 11", "//": "DarkPineColor",
          "deviation": 50
        }
      }
    ]
  }
}
```

For each replaced color occurence, selects and generates a random color from a provided set.

#### Spotty operator
```json
{
  "FFFFFF": {
    "$spotty": {
      "base": "111, 161, 255",
      "spots": [
        {
          "density": 50000,
          "radius": 2.5,
          "color": {
            "$randSat": {
              "base": "255, 255, 255",
              "deviation": 50
            }
          }
        }
      ]
    }
  }
}
```

When initialized, for each spot type creates [density] spots with random positions and radius [radius] in area from [-100;-100;-100] to [100;100;100].
Blocks outside spots are colored to [base] color (remember it can also be made "$randSat" or other color operator).

### Perlin noise operator

Sample 1:
```json
{
  "FFFFFF": {
    "$perlin": {
      "easing": "p2",
      "div": 4,
      "octaves": 4,
      "a": "136, 148, 166",
      "b": "200, 212, 226"
    }
  }
}
```

Perlin noise is very powerful tool to make varied camouflages. It has some important parameters: 
1. Scale of noise ("div")
2. Count of octaves ("octaves")
3. Easing mode ("easing")

The simplest one is a scale (parameter "div"). Remember: never set it to 1, or camo will be just plain color. Base perlin noise with 1 octave is similar to many "worms" (you can google some pictures). The "div" parameter is a thickness these worms (in blocks).  

When increasing count of octaves, the pattern becomes more sophisticated. With 1 octave, it is plain "worms". With more octaves, the "labyrinth" becomes trickier and looks more random.  

Easing is required to handle color transitions. Easing can be chosen from predefined set. Here is currently supported easing modes:

* "linear"
* "quintic"
* "p2"
* "p3"
* "p4"
* "expo10"
* "expo10In"
* "c1"
* "c2"

You shouldn't use linear function (it will lead to strange color blends).  

I can divide this colors to groups by some factors.  

First factor is symmetry:  
"linear", "quintic", "expo10", "c1" are symmetric (values of color "a", and color "b" are equal, their quantity in camo will be pretty equal).
"p2", "p3", "p4", "expo10In", "c2" are non-symmetric and value of color "b" is less than color "a". You can use them, when you have some base color ("a"), and some contrasted color "b", that you want to see in small amount.  
        
Second factor is level of amplification (if it is low, colors of camo will be distributed equally between color "a" and "b"; if it is high, colors of camo will be very close to color "a" or "b")  
"linear" is most non-amplifying, while "c1" and "c2" are most amplifying ("c1" and "c2" return only colors "a" and "b", without any blends). And here is their ranking from least to most amplifying:
1. linear
2. p2
3. p3
4. p4
5. quintic
6. expo10, expo10In
7. c1, c2
