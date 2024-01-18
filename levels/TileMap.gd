extends TileMap

export(int) var map_w = 80
export(int) var map_h = 50
export(int) var min_room_size = 8
export(float, 0.2, 0.5) var min_room_factor = 0.4

enum Tiles { GROUND, TREE, WATER, ROOF }

var tree = {}
var leaves = []
var leaf_id = 0
var rooms = []
